async function farmerUpdateProfile() {
    event.preventDefault();

    const form = document.getElementById("editForm");
    const formData = new FormData(form);
    const farmerId = document.getElementById("FarmerId").value;

    try {
        const response = await fetch(`/api/farmers/edit/${farmerId}`, {
            method: 'PATCH',
            body: formData,
        });

        if (!response.ok) {
            throw new Error('Failed to update farmer.');
        }

        const data = await response.json();

        if (data.success) {
            alert('Farmer update successfully!');
            window.location.href = `/api/farmers/profile/${farmerId}`;
        } else {
            alert('Failed to update farmer!');
        }
    } catch (error) {
        console.error('Error', error);
        alert('An error occurred!');
    }
}