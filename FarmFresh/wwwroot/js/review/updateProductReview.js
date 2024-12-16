async function productReviewUpdate() {
    event.preventDefault();

    const form = document.getElementById("editForm");
    const formData = new FormData(form);
    const reviewId = document.getElementById("ReviewId").value;
    const baseUrl = window.location.origin;

    try {
        const response = await fetch(`${baseUrl}/api/reviews/editproductreview/${reviewId}`, {
            method: 'PATCH',
            body: formData
        });

        if (!response.ok) {
            throw new Error('Failed to update review');
        }

        const data = await response.json();

        if (data.success) {
            alert('Review updated successfully!');
            window.location.href = baseUrl;
        }
        else {
            alert('Failed to update review.')
        }
    } catch (e) {
        console.error('Error updating review:', e);
        alert('An error occurred while updating the review.');
    }
}