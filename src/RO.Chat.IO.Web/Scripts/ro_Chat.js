$(function () {
    var rochat = $.connection.rO_ChatHub;

    rochat.client.novaMensagem = function (mensagemViewModel) {

        if (model.meuUsuario() == mensagemViewModel.Id_Usuario_Mensagem &&
            model.usuarioEscolhido() == mensagemViewModel.Id_Usuario_Destino) {
            window.focus();
            return;
        }

        if (model.usuarioEscolhido() == mensagemViewModel.Id_Usuario_Mensagem) {

            model.adicionarNovaMensagem(mensagemViewModel.Texto,
                mensagemViewModel.DataMensagem,
                mensagemViewModel.Id_Usuario_Mensagem,
                mensagemViewModel.NomeUsuario,
                mensagemViewModel.Login,
                0
            );

            $(".msg_history").animate({ scrollTop: $(".msg_history")[0].scrollHeight }, 1000);

        }
        else
        {
            toastr["info"]('Mensagem enviada por ' + mensagemViewModel.NomeUsuario );
        }

        window.focus();

    };

    rochat.client.novoGrupo = function (grupo) {
                
        toastr["info"]('Adicionado novo grupo ' + grupo.Nome + '!');
        
        model.usuarios.push(new adicionarUsuario(grupo));            

        window.focus();

    };

    rochat.client.novaMensagemGrupo = function (mensagemViewModel) {

        if (model.usuarioEscolhido() == mensagemViewModel.Id_Grupo &&
            mensagemViewModel.Id_Usuario_Mensagem == model.meuUsuario()) {
            window.focus();
            return
        }

        if (model.usuarioEscolhido() == mensagemViewModel.Id_Grupo) {

            model.adicionarNovaMensagem(mensagemViewModel.Texto,
                mensagemViewModel.NomeUsuario + ' - ' +  mensagemViewModel.DataMensagem,
                mensagemViewModel.Id_Usuario_Mensagem,
                mensagemViewModel.NomeUsuario,
                mensagemViewModel.Login,
                0
            );

            $(".msg_history").animate({ scrollTop: $(".msg_history")[0].scrollHeight }, 1000);
        }
        else {
            toastr["info"]('Mensagem enviada para o grupo ' + mensagemViewModel.Nome_Grupo);
        }

        window.focus();
    };

    rochat.client.informarQuemConectou = function (usuarioViewModel) {

        if (model.meuUsuario() != usuarioViewModel.Id_Usuario) {
            toastr["info"](usuarioViewModel.NomeUsuario + ' conectou!');
        }

        for (var i = 0; i < model.usuarios.length; i++) {
            if (model.usuario[i].Id_Usuario == usuarioViewModel.Id_Usuario)
                return;
        }

        model.usuarios.push(new adicionarUsuario(usuarioViewModel));                       

    };

    rochat.client.informarQuemSaiu = function (usuarioViewModel) {

        toastr["info"](usuarioViewModel.NomeUsuario + ' saiu!');

    };

    rochat.client.marcarUsuarioLogado = function (id) {
        model.meuUsuario(id);
    };

    $.connection.hub.start().done(function () {
        toastr["success"]("Connectado com sucesso!");
        model.obterUsuarios();

    });



    $('ul#id_lista_usuarios').on('click', 'li', function () {

        $('.list-group-item').each(function () {
            $(this).removeClass('item-active');
        });

        $('#' + this.id).addClass('item-active');

        model.usuarioEscolhido(this.id);
        model.ehGrupo(this.attributes['data-grupo'] !== undefined ? 1 : 0);
        model.obterMensagensPorUsuario();

    });


    $('#enviar_Mensagem').on('click', function () {

        if (model.mensagem().length === 0) {
            toastr["warning"]("Texto da mensagem em branco!");
            this.focus();
            return;
        }

        if (model.usuarioEscolhido().length === 0) {
            toastr["warning"]("Nenhum usuário ou grupo selecionado!");
            this.focus();
            return;
        }

        model.adicionarNovaMensagem(model.mensagem(), (new Date()).toISOString().match(/^[0-9]{4}\-[0-9]{2}\-[0-9]{2}/)[0].split('-').reverse().join('/') , '', '', '', 1);
        model.enviarMensagem();

    });

    $('#incluir_grupo').on('click', function () {
        model.adicionarGrupo($('#id_nome_grupo').val());
        $('#id_adicionar_grupo').modal('hide');
        $('#id_nome_grupo').val('');
    });

    var Model = function () {
        var self = this;
        self.usuarios = ko.observableArray([]);
        self.meuUsuario = ko.observable("");
        self.mensagens = ko.observableArray([]);
        self.mensagem = ko.observable("");
        self.usuarioEscolhido = ko.observable("");
        self.ehGrupo = ko.observable("");
        self.query = ko.observable("");


        self.usuariosFiltro = ko.computed(function () {
            var query = this.query().toLowerCase();
            if (!query) {
                return self.usuarios();
            } else {

                return ko.utils.arrayFilter(self.usuarios(), function (user) {
                    var displayName = user.nome_usuario().toLowerCase();
                    return displayName.includes(self.query().toLowerCase());
                });
            }
        }, self);
    };


    Model.prototype = {

        obterUsuarios: function () {
            var self = this;

            rochat.server.obterUsuarios().done(function (result) {
                self.usuarios.removeAll();
  
                for (var i = 0; i < result.length; i++) {
                    self.usuarios.push(new adicionarUsuario(result[i]));
                }
            });

        },

        adicionarGrupo: function (nome_grupo) {
            var self = this;

            rochat.server.adicionarNovoGrupo(nome_grupo).done(function (result) {

                if (result > 0)
                    toastr["success"]('Grupo adicionado com sucesso!');

            });

        },

        obterMensagensPorUsuario: function () {
            var self = this;

            if (self.ehGrupo() === 1) {

                rochat.server.obterMensagensPorGrupo(self.usuarioEscolhido()).done(function (result) {
                    self.mensagens.removeAll();
                    for (var i = 0; i < result.length; i++) {

                        self.mensagens.push(new adicionarMensagem(result[i].Texto,
                            (result[i].Id_Usuario_Mensagem != self.meuUsuario() ? result[i].NomeUsuario + ' - ' : '' )+  result[i].DataMensagem,
                            result[i].Id_Usuario_Mensagem,
                            result[i].NomeUsuario,
                            result[i].Login,
                            result[i].Id_Usuario_Mensagem === self.meuUsuario() ? 1 : 0
                        ));
                    }

                    $(".msg_history").animate({ scrollTop: $(".msg_history")[0].scrollHeight }, 1000);

                });
            }
            else {
                rochat.server.obterMensagensPorUsuario(self.usuarioEscolhido()).done(function (result) {
                    self.mensagens.removeAll();
                    for (var i = 0; i < result.length; i++) {

                        self.mensagens.push(new adicionarMensagem(result[i].Texto,
                            result[i].DataMensagem,
                            result[i].Id_Usuario_Mensagem,
                            result[i].NomeUsuario,
                            result[i].Login,
                            result[i].Id_Usuario_Mensagem === self.meuUsuario() ? 1 : 0
                        ));
                    }

                    $(".msg_history").animate({ scrollTop: $(".msg_history")[0].scrollHeight }, 1000);

                });

            }

        },


        adicionarNovaMensagem: function (msg, dataMsg, meuUsu, nomeUsu, login, minhaMensagem)
        {
            var self = this;

            self.mensagens.push(new adicionarMensagem(
                msg,
                dataMsg,
                meuUsu,
                nomeUsu,
                login,
                minhaMensagem
            ));

            $(".msg_history").animate({ scrollTop: $(".msg_history")[0].scrollHeight }, 1000);
        },


        enviarMensagem: function () {
            var self = this;

            if (self.ehGrupo() === 1)
                rochat.server.enviarMensagemGrupo(self.usuarioEscolhido() , self.mensagem());
            else
                rochat.server.enviarMensagemUsuario(self.usuarioEscolhido(), self.mensagem());


            self.mensagem('');
        },

    };

    function adicionarUsuario(novoUsuarioGrupo)
    {
        var self = this;
        self.id = ko.observable(novoUsuarioGrupo.Id);
        self.nome = ko.observable(novoUsuarioGrupo.Nome);
        self.nome_chave = ko.observable(novoUsuarioGrupo.NomeChave);
        self.nome_usuario = ko.observable(novoUsuarioGrupo.NomeUsuario);
        self.ehGrupo = ko.observable(novoUsuarioGrupo.IsGrupo);
    }

    function adicionarMensagem(msg, dataMensagem, idUsuarioMensagem, usuario, login, userSelf) {
        var self = this;
        self.msg = ko.observable(msg);
        self.dataMensagem = ko.observable(dataMensagem);
        self.idUsuarioMensagem = ko.observable(idUsuarioMensagem);
        self.usuario = ko.observable(usuario);
        self.login = ko.observable(login);
        self.userSelf = ko.observable(userSelf);
    }


    var model = new Model();
    ko.applyBindings(model);
    
});