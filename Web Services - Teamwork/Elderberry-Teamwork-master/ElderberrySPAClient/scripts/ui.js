define(['jquery', 'controller'], function ($, controller) {
    'use strict';

    var uiHandler = (function () {
        function loadDefaultTemplate() {
            $('#main-container').load('templates/default-page.html', function () {
                $('#user-greeting').append(sessionStorage.getItem('userName') || 'anonymous');
                if (!controller.isUserLogged()) {
                    $('#btn-logout').hide();
                    $('#btn-check-code').hide();
                    $('#btn-view-registered-codes').hide();
                } else {
                    $('#btn-login').hide();
                    $('#btn-register').hide();
                }
            });
        }

        function loadLoginTemplate() {
            $('#main-container').load('templates/login.html');
        }

        function loadRegisterTemplate() {
            $('#main-container').load('templates/register.html');
        }

        function loadCheckCodeTemplate() {
            $('#main-container').load('templates/check-code.html');
        }

        function loadViewRegisteredCodesTemplate() {
            $('#main-container').load('templates/view-registered-codes.html');
        }

        function loadAdminPanel() {
            $('#main-container').load('templates/admin.html');
        }

        function loadAddCodeTemplate() {
            $('#main-container').load('templates/add-code.html');
        }

        return {
            loadDefaultTemplate: loadDefaultTemplate,
            loadLoginTemplate: loadLoginTemplate,
            loadRegisterTemplate: loadRegisterTemplate,
            loadCheckCodeTemplate: loadCheckCodeTemplate,
            loadViewRegisteredCodesTemplate: loadViewRegisteredCodesTemplate,
            loadAdminPanel: loadAdminPanel,
            loadAddCodeTemplate: loadAddCodeTemplate
        };
    }());

    return uiHandler;
});