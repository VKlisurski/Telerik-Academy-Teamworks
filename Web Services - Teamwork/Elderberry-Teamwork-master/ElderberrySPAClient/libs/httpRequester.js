/* global define */
define(['jquery', 'q'], function ($, Q) {
    'use strict';

    var httpRequester = (function () {
        var getJSON = function (url, headers) {
            var deferred = Q.defer();

            $.ajax({
                url: url,
                type: 'GET',
                contentType: 'application/x-www-form-urlencoded',
                headers: headers || {},
                //timeout: 10000,
                success: function (success) {
                    deferred.resolve(success);
                },
                error: function (error) {
                    deferred.reject(error);
                }
            });

            return deferred.promise;
        };

        var postJSON = function (url, data, headers) {
            var deferred = Q.defer();

            $.ajax({
                url: url,
                type: 'POST',
                data: data || {},
                contentType: 'application/x-www-form-urlencoded',
                headers: headers || {},
                //timeout: 10000,
                success: function (success) {
                    deferred.resolve(success);
                },
                error: function (error) {
                    deferred.reject(error);
                }
            });

            return deferred.promise;
        };

        var putJSON = function (url, sessionKey) {
            var deferred = Q.defer();

            $.ajax({
                url: url,
                type: 'PUT',
                contentType: 'application/json',
                headers: {
                    'X-SessionKey': sessionKey
                },
                timeout: 5000,
                success: function (success) {
                    deferred.resolve(success);
                },
                error: function (error) {
                    deferred.reject(error);
                }
            });

            return deferred.promise;
        };

        var deleteJSON = function (url, data, headers) {
            var deferred = Q.defer();

            $.ajax({
                url: url,
                type: 'DELETE',
                contentType: 'application/x-www-form-urlencoded',
                headers: headers || {},
                data: data || {},
//                timeout: 5000,
                success: function (success) {
                    deferred.resolve(success);
                },
                error: function (error) {
                    deferred.reject(error);
                }
            });

            return deferred.promise;
        };

        return {
            getJSON: getJSON,
            postJSON: postJSON,
            putJSON: putJSON,
            deleteJSON: deleteJSON
        };
    }());

    return httpRequester;
});