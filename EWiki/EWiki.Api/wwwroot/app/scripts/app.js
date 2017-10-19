'use strict';
var ewikiBD = angular.module('ewikibd', [
        'oc.lazyLoad',
        'ngIdle',
        'ngSanitize',
        'cloudinary',
        'ui.bootstrap',
        'ui.router'
]);

var baseUrl = $('base').attr('href');
var appBaseUrl = baseUrl + '/';
var apiBaseUrl = appBaseUrl.replace('app', 'api');