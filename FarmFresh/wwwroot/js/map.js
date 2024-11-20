function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 42.7339, lng: 25.4858 }, 
        zoom: 7
    });

    const farmersLocations = window.farmersLocations; 

    farmersLocations.forEach(location => {
        if (typeof location.latitude === 'number' && typeof location.longitude === 'number') {
            const marker = new google.maps.Marker({
                position: { lat: location.latitude, lng: location.longitude },
                map: map,
                title: location.title,
            });

            marker.addEventListener('click', () => {

                infoWindow.setContent(`
                <div>
                    <h3>${location.title}</h3>
                    <p>Latitude: ${location.latitude}</p>
                    <p>Longitude: ${location.longitude}</p>
                </div>
            `);
                infoWindow.open(map, marker);
            });

        } else {
            console.error('Invalid latitude or longitude value:', location);
        }
}

function loadGoogleMapsAPI() {
    var script = document.createElement('script');
    script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyCDU8bRvELoiVGRLP7mGqP3eWlREBE2z-w&callback=initMap";
    script.async = true;
    script.defer = true;
    document.head.appendChild(script);
}
document.addEventListener('DOMContentLoaded', function () {

    const faqs = document.querySelectorAll('.faq-question');

    faqs.forEach(faq => {

        faq.addEventListener('click', () => {

            const answer = faq.nextElementSibling;

            if (answer.style.display === 'none' || answer.style.display === '') {
                answer.style.display = 'block';
            }
            else {
                answer.style.display = 'none';
            }
        });
    });

    loadGoogleMapsAPI();
});