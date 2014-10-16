define(['httpRequester', 'q'], function (httpRequester, q) {
    'use strict';

    var apiManager = (function () {
        function loginHandler(requestUrl, username, password) {
            //validation
            var url, data;

            url = requestUrl + '/Token';
            data = {
                grant_type: 'password',
                username: username,
                password: password
            };
            httpRequester.postJSON(url, data)
                .then(function (result) {
                    sessionStorage.setItem('authToken', result.access_token);
                    sessionStorage.setItem('userName', result.userName);
                    document.getElementById('info').innerHTML = 'Logged in successfully!';
                    //window.location = '#/';
                }, function (error) {
                    document.getElementById('info').innerHTML = 'Error: ' + error.responseJSON.error_description;
                });
        }

        function registerHandler(requestUrl, email, password, confirmPassword) {
            //validation
            var authCode, url, data, header;

            url = requestUrl + '/api/Account/Register';
            data = {
                Email: email,
                Password: password,
                ConfirmPassword: confirmPassword
            };
            httpRequester.postJSON(url, data)
                .then(function (result) {
                    document.getElementById('info').innerHTML = 'Registration successful!';
                    window.location = '#/login';
                }, function (error) {
                    document.getElementById('info').innerHTML = 'Error: ' + error.responseJSON.Message;
                });
        }

        function logoutUser(requestURL, authToken) {
            var url, headers;

            url = requestURL + '/api/account/logout';
            headers = {
                'Authorization': authToken
            };
            httpRequester.postJSON(url, undefined, headers)
                .then(function (result) {
                    document.getElementById('info').innerHTML = 'Logged out successfully!';
                }, function (error) {
                    document.getElementById('info').innerHTML = error.responseJSON.Message;
                });
        }

        function checkCode(requestURL, authToken, code) {
            var url, data, headers;

            url = requestURL + '/api/gamecode/create';
            data = {
                value: code
            };
            headers = {
                'Authorization': authToken
            };

            httpRequester.postJSON(url, data, headers)
                .then(function (result) {
                    document.getElementById('info').innerHTML = result;
                }, function (error) {
                    document.getElementById('info').innerHTML = error.responseJSON.Message;
                })
                .done();
        }

        function checkRegisteredCodes(requestURL, authToken) {
            var url, data, headers, deferred;

            deferred = q.defer();

            url = requestURL + '/api/gamecode/getusercodes';
            headers = {
                'Authorization': authToken
            };

            httpRequester.getJSON(url, headers)
                .then(function (result) {
                    deferred.resolve(result);
                }, function (error) {
                    deferred.resolve(error);
                })
                .done();

            return deferred.promise;
        }

        function adminViewAllCodes(requestURL, authToken) {
            var url, data, headers, deferred;

            deferred = q.defer();

            url = requestURL + '/api/admin/allcodes';
            headers = {
                'Authorization': authToken
            };

            httpRequester.getJSON(url, headers)
                .then(function (result) {
                    deferred.resolve(result);
                }, function (error) {
                    deferred.resolve(error);
                })
                .done();

            return deferred.promise;
        }

        function adminViewAllUsers(requestURL, authToken) {
            var url, data, headers, deferred;

            deferred = q.defer();

            url = requestURL + '/api/admin/allusers';
            headers = {
                'Authorization': authToken
            };

            httpRequester.getJSON(url, headers)
                .then(function (result) {
                    deferred.resolve(result);
                }, function (error) {
                    deferred.resolve(error);
                })
                .done();

            return deferred.promise;
        }

        function deleteUser(requestURL, username, authToken) {
            var url, data, headers;

            url = requestURL + '/api/admin/deleteuser';
            data = {
                username: username
            };
            headers = {
                'Authorization': authToken
            };

            httpRequester.deleteJSON(url, data, headers)
                .then(function (result) {
                    document.getElementById('info').innerHTML = result;
                }, function (error) {
                    document.getElementById('info').innerHTML = error.responseJSON.Message;
                })
                .done();
        }

        function addCode(requestURL, code, isWinning, authToken) {
            var url, data, headers;

            url = requestURL + '/api/admin/addcode';
            data = {
                value: code,
                isWinning: isWinning
            };
            headers = {
                'Authorization': authToken
            };

            httpRequester.postJSON(url, data, headers)
                .then(function (result) {
                    document.getElementById('info').innerHTML = result;
                }, function (error) {
                    document.getElementById('info').innerHTML = error.responseJSON.Message;
                })
                .done();
        }

        return{
            login: loginHandler,
            register: registerHandler,
            logout: logoutUser,
            checkCode: checkCode,
            checkRegisteredCodes: checkRegisteredCodes,
            adminViewAllCodes: adminViewAllCodes,
            adminViewAllUsers: adminViewAllUsers,
            deleteUser: deleteUser,
            addCode: addCode
        };
    }());

    return apiManager;
});