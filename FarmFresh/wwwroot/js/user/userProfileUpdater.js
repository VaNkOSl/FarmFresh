function updateUserProfile() {
  event.preventDefault();

    const form = document.getElementById("editForm");
    const formData = new FormData(form);

    const userId = document.getElementById("UserId").value;

    fetch(`/api/account/edit/${userId}`, {
        method: 'PATCH',
        body: formData,
    })
    .then(response => {
        if (response.ok) {
            return response.json(); 
        } else {
            throw new Error('Failed to update user');
        }
    })
        .then(data => {
            if (data.success) {
                alert('User update successfully!');
                window.location.href = `/api/account/profile/${userId}`;
            } else {
                alert('Failed to update user!');
            }
        })
        .catch(error => {
            console.log('Error', error);
            alert('An error occured!');
        })
}