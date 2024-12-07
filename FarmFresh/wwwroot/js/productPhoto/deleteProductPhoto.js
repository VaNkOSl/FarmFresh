document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.close-button').forEach(button => {
        button.addEventListener('click', function () {
            const photoId = this.getAttribute('data-photo-id');

            if (photoId && confirm('Are you sure you want to delete this photo?')) {
                const baseUrl = window.location.origin; 
                const apiUrl = `${baseUrl}/api/productphoto/delete/${photoId}`;

                fetch(apiUrl, {
                    method: 'DELETE', 
                })
                    .then(response => {
                        if (response.ok) {
                            return response.json();
                        } else {
                            throw new Error('Failed to delete photo.');
                        }
                    })
                    .then(data => {
                        if (data.success) {
                            this.closest('.image-container').remove();
                        } else {
                            alert('Error deleting photo.');
                        }
                    })
                    .catch(error => alert('Error: ' + error));
            }
        });
    });
});

