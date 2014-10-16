(function () {
    'use strict';

    require.config({
        paths: {
            'jquery': '../libs/jquery-2.1.1',
            'q': '../libs/q',
            'sammy': '../libs/sammy',
            'underscore': '../libs/underscore',
            'handlebars': '../libs/handlebars-v1.3.0',
            'httpRequester': '../libs/httpRequester',
            'apiManager': 'api-manager',
            'controller': 'controller',
            'ui': 'ui',
            'bootstrap': 'https://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js'
        },
        shim: {
            'handlebars': {
                exports: 'Handlebars'
            },
            'bootstrap': {
                'deps': ['jquery']
            }
        }
    });

    require(['jquery', 'sammy', 'handlebars', 'apiManager', 'controller', 'ui'], function ($, sammy, Handlebars, apiManager, controller, ui) {

//        var serverURL = 'http://elderberrylottery.apphb.com';
        var serverURL = 'http://localhost:32564';
        controller.setRequestsURL(serverURL);
        controller.setDOMEvents();

        var app = sammy('#main-container', function () {
            this.get('#/', function () {
                ui.loadDefaultTemplate();
            });

            this.get('#/login', function () {
                ui.loadLoginTemplate();
            });

            this.get('#/register', function () {
                ui.loadRegisterTemplate();
            });

            this.get('#/check-code', function () {
                ui.loadCheckCodeTemplate();
            });

            this.get('#/view-registered-codes', function () {
                ui.loadViewRegisteredCodesTemplate();
            });

            this.get('#/admin', function () {
               ui.loadAdminPanel();
            });

            this.get('#/admin/add-code', function(){
               ui.loadAddCodeTemplate();
            });
        });

        $(function () {
            app.run('#/');
        });
    });
}());