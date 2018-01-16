$(document).ready(function () {

    let map;
    let marker;

    function initMap(latitude, longtitude) {
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 15,
            center: { lat: latitude, lng: longtitude }
        });
        marker = new google.maps.Marker({
            position: { lat: latitude, lng: longtitude },
            map: map
        });
    }

    //Milwaukee Init
    //43.0389° N, 87.9065° W
    //initMap(43.0389, -87.9065);

    //Waukesha Init
    //43.0117° N, 88.2315° W
    initMap(43.0117, -88.2315);

    //$("#addressCreateForm :input").change(function () {
    //    $("#submitButton").fadeOut(function () {
    //        $("#map").fadeOut();
    //        $("#search").fadeIn();
    //    });
    //});

    let errorMessage = [];
    $('#search')
        .click(function (event) {
            $('#validationErrors').hide();
            errorMessage = [];
            let isValidated = validate();
            if (isValidated) {
                event.preventDefault();
                let streetAddressOne = $('#streetAddress').val();
                let cityName = cleanCode($('#city').val());
                let stateId = cleanCode($('#StateId').val());
                let stateAbbreviation = $('option[value=' + stateId + ']').text();
                let streetAddressElements = streetAddressOne.split(" ");
                let googleMapUrl = 'https://maps.googleapis.com/maps/api/geocode/json?address=';
                for (let i = 0; i < streetAddressElements.length; i++) {
                    googleMapUrl += cleanCode(streetAddressElements[i]) + '+';
                }
                googleMapUrl += cityName + '+';
                googleMapUrl += stateAbbreviation;
                googleMapUrl += '&key=AIzaSyD3F02Dr7BSQRR48YgU8akdwdR-9FsXp3w';
                let lat;
                let lng;

                console.log(googleMapUrl);
                $.getJSON(googleMapUrl)
                .done(function (data) {
                    let location = data.results[0].geometry.location;
                    lat = location.lat;
                    lng = location.lng;
                    $('#lat').val(lat);
                    $('#lng').val(lng);
                    $("#map").fadeOut(function () {
                        if (typeof map !== 'undefined')
                        {
                            moveMarker(lat,lng);
                        }
                        else {
                            initMap(lat, lng);
                        }
                        $("#map").fadeIn();
                    });
                });
            }
            else {
                if (errorMessage.length > 0) {
                    let errorMessageHtml = '<ul>\n';
                    for (let i = 0; i < errorMessage.length; i++) {
                        errorMessageHtml += '<li>' + errorMessage[i] + '</li>\n';
                    }
                    errorMessageHtml += '</ul>';
                    $('#validationErrors').fadeOut(function () {
                        $('#validationErrors > div').html(errorMessageHtml);
                        $('#validationErrors').fadeIn();
                    });
                    event.preventDefault();
                }
            }
        });

    function moveMarker(latitude, longtitude)
    {
        map.setCenter({ lat: latitude, lng: longtitude });
        marker.setPosition({ lat: latitude, lng: longtitude });
    }

    function cleanCode(c) {
        return c.replace(/[^A-Za-z0-9_]/g, "");
    }

    function validate() {
        isValid = true;
        if ($('#address').val() == "") {
            errorMessage.push("You must enter a street address.");
            isValid = false;
        }
        if ($('#city').val() == "") {
            errorMessage.push("You must enter a city.");
            isValid = false;
        }
        return isValid;
    }

    //NEW STUFF STARTS HERE///////////////////////////////////////////////////////////////

    // Note: This example requires that you consent to location sharing when
    // prompted by your browser. If you see the error "The Geolocation service
    // failed.", it means you probably did not give permission for the browser to
    // locate you.
    //var map, infoWindow;
    //function initMap() {
    //    map = new google.maps.Map(document.getElementById('map'), {
    //        center: { lat: -34.397, lng: 150.644 },
    //        zoom: 6
    //    });
    //    infoWindow = new google.maps.InfoWindow;

    //    // Try HTML5 geolocation.
    //    if (navigator.geolocation) {
    //        console.log("Geolacation activated");
    //        navigator.geolocation.getCurrentPosition(function (position) {
    //            var pos = {
    //                lat: position.coords.latitude,
    //                lng: position.coords.longitude
    //            };

    //            infoWindow.setPosition(pos);
    //            infoWindow.setContent('Location found.');
    //            infoWindow.open(map);
    //            map.setCenter(pos);
    //        }, function () {
    //            handleLocationError(true, infoWindow, map.getCenter());
    //        });
    //    } else {
    //        // Browser doesn't support Geolocation
    //        handleLocationError(false, infoWindow, map.getCenter());
    //    }
    //}

    //function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    //    infoWindow.setPosition(pos);
    //    infoWindow.setContent(browserHasGeolocation ?
    //                          'Error: The Geolocation service failed.' :
    //                          'Error: Your browser doesn\'t support geolocation.');
    //    infoWindow.open(map);
    //}

    //initMap();

    //MORE CURRENT LOCATION STUFF///////////////////////////////////////////////////////////

    $('#locationButton').click(function () {
        console.log("clicked");
        getLocation();
    });

    function getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(success, error);
        } else {
            $("#locationError").innerHTML = "Geolocation is not supported by this browser.";
        }
    }

    function success() {
        console.log("success");
    }

    function error() {
        console.log("success");
    }


    //////////////////////////////////////////////////////////////////////////////////////

});
