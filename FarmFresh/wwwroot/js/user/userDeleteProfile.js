function deleteUserProfile() {
    if (confirm('Are you sure you want to delete your profile?')) {
        var formData = new FormData(document.getElementById("deleteUser"));
        var id = formData.get("id");

        fetch(`/api/account/delete/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: id })
        })
            .then(response => {
                if (response.ok) {
                    alert('Profile deleted successfully.');
                    window.location.href = '/';
                } else {
                    alert('Error deleting profile.');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Something went wrong.');
            });
    }
}