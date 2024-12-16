function deleteReview(reviewId) {
    if (confirm('Are you sure you want to delete this review?')) {
        fetch(`/api/reviews/delete/${reviewId}`, {
            method: 'DELETE',
        })
            .then(response => {
                if (response.ok) {
                    alert('Review deleted successfully.');
                    window.location.reload();
                } else {
                    alert('Error deleting review. Please try again later.');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Something went wrong.');
            });
    }
}

function editReview(reviewId) {
    document.getElementById(`review-display-${reviewId}`).classList.add('d-none');
    document.getElementById(`edit-form-${reviewId}`).classList.remove('d-none');
}

function cancelEdit(reviewId) {
    document.getElementById(`edit-form-${reviewId}`).classList.add('d-none');
    document.getElementById(`review-display-${reviewId}`).classList.remove('d-none');
}