function initAutocomplete() {
    const locationInput = document.getElementById('locationInput');
    const suggestionsContainer = document.getElementById('suggestions');
    const autocompleteService = new google.maps.places.AutocompleteService();

    locationInput.addEventListener('input', () =>
        handleInput(locationInput, suggestionsContainer, autocompleteService)
    );
}

function handleInput(locationInput, suggestionsContainer, autocompleteService) {
    const query = locationInput.value;

    if (query.length > 2) {
        autocompleteService.getPlacePredictions(
            { input: query, componentRestrictions: { country: 'bg' } },
            (predictions, status) => {
                if (status === google.maps.places.PlacesServiceStatus.OK) {
                    displaySuggestions(predictions, locationInput, suggestionsContainer);
                }
            }
        );
    } else {
        clearSuggestions(suggestionsContainer);
    }
}

function displaySuggestions(predictions, locationInput, suggestionsContainer) {
    clearSuggestions(suggestionsContainer);

    predictions.forEach(prediction => {
        const suggestionItem = document.createElement('div');
        suggestionItem.className = 'list-group-item list-group-item-action';
        suggestionItem.textContent = prediction.description;

        suggestionItem.addEventListener('click', () => {
            locationInput.value = prediction.description;
            getPlaceDetails(prediction.place_id);
            clearSuggestions(suggestionsContainer);
        });

        suggestionsContainer.appendChild(suggestionItem);
    });
}

function clearSuggestions(suggestionsContainer) {
    suggestionsContainer.innerHTML = '';
}

function getPlaceDetails(placeId) {
    const placesService = new google.maps.places.PlacesService(document.createElement('div'));

    placesService.getDetails({ placeId: placeId }, (place, status) => {
        if (status === google.maps.places.PlacesServiceStatus.OK) {
            document.getElementById('Latitude').value = place.geometry.location.lat();
            document.getElementById('Longitude').value = place.geometry.location.lng();
        }
    });
}

function loadGoogleMapsAPI(callback) {
    const script = document.createElement('script');
    script.src =
        "https://maps.googleapis.com/maps/api/js?key=AIzaSyCDU8bRvELoiVGRLP7mGqP3eWlREBE2z-w&libraries=places&callback=" +
        callback;
    script.async = true;
    script.defer = true;
    document.head.appendChild(script);
}

window.initAutocomplete = initAutocomplete;

document.addEventListener('DOMContentLoaded', () => {
    loadGoogleMapsAPI('initAutocomplete');
});