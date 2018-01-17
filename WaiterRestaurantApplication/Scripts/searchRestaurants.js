$(document).ready(function () {

    let map;
    let userMarker;

    //User's info window
    var infowindow = new google.maps.InfoWindow({
        content: 'You are here'
    });

    //display the map
    function initMap(latitude, longtitude) {
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 15,
            center: { lat: latitude, lng: longtitude }
        });
        userMarker = new google.maps.Marker({
            position: { lat: latitude, lng: longtitude },
            icon: {
                path: google.maps.SymbolPath.CIRCLE,
                scale: 10
            },
            map: map
        });
        userMarker.addListener('click', function () {
            infowindow.open(map, userMarker);
        });
    }

    //Milwaukee Init
    //43.0389° N, 87.9065° W
    //initMap(43.0389, -87.9065);

    //Waukesha Init
    //43.0117° N, 88.2315° W
    initMap(43.0117, -88.2315);

    //Initial action to take after the diner does a search by address
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

    //move the user's map marker
    function moveMarker(latitude, longtitude)
    {
        map.setCenter({ lat: latitude, lng: longtitude });
        userMarker.setPosition({ lat: latitude, lng: longtitude });
    }

    //clean up the string
    function cleanCode(c) {
        return c.replace(/[^A-Za-z0-9_]/g, "");
    }

    //validate the form data
    function validate() {
        isValid = true;
        if ($('#streetAddress').val() == "") {
            errorMessage.push("You must enter a street address.");
            isValid = false;
        }
        if ($('#city').val() == "") {
            errorMessage.push("You must enter a city.");
            isValid = false;
        }
        return isValid;
    }

    //set up the restaurant markers
    let markers = [];
    $(".marker").each(function () {
        let mapData = $(this).data();
        //addMarker(mapData.lat, mapData.lng, mapData.count);
        let markerObject = {};
        markerObject.lat = parseFloat(mapData.lat);
        markerObject.lng = parseFloat(mapData.lng);
        markerObject.restaurantName = mapData.name;
        markerObject.waitTime = mapData.waittime;
        markerObject.restaurantId = mapData.restaurantid;
        markers.push(markerObject);
    });
    addMarkers(markers);

    //display the markers
    function addMarkers(markers) {
        for (let i = 0; i < markers.length; i++)
        {
            let infowindow = new google.maps.InfoWindow({
                content:
                    '<h4>' + markers[i].restaurantName + '</h4>' +
                    '<p><strong>Stated Wait Time:</strong> ' + markers[i].waitTime + ' minutes</p>' +
                    '<p><strong>Average Wait Time:</strong> ' + '45' + ' minutes</p>' +
                    '<p><strong>Wait Rate:</strong> ' + '45' + '%</p>' +
                    '<p><a href="/TableVisit/Create?restaurantId=' + markers[i].restaurantId + '&isHostEntry=false" class="btn btn-primary">Get On The List!</a></p>'

            });
            let restaurantMarker = new google.maps.Marker({
                position: { lat: markers[i].lat, lng: markers[i].lng },
                map: map
            });
            restaurantMarker.addListener('click', function () {
                infowindow.open(map, restaurantMarker);
            });
        }
    }

    //When the user clicks the 'Search by my location' button
    $('#locationButton').click(function () {
        //Use HTML5 to get the user's current location
        if (navigator.geolocation) {
            console.log("Geolacation activated");
            navigator.geolocation.getCurrentPosition(function (position) {
                var pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                //move the map marker to the user's location
                $("#map").fadeOut(function () {
                    moveMarker(pos.lat, pos.lng);
                    $("#map").fadeIn();
                });

            }, function () {
                handleLocationError(true);
            });
        } else {
            // Browser doesn't support Geolocation
            console.log("Your browser doesn\'t support geolocation.");
            handleLocationError(false);
        }
    });

    function handleLocationError(browserHasGeolocation) {
        message = browserHasGeolocation ? 'Error: The Geolocation service failed.' : 'Error: Your browser doesn\'t support geolocation.';
        alert(message);
    }

});
