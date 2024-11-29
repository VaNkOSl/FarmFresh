function deleteCategory(id) {
    if (confirm("Are you sure you want to delete this category?")) {
        fetch(`delete/${id}`, { 
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()  
            }
        })
            .then(response => {
                if (response.ok) {
                    let row = document.getElementById(`category-row-${id}`);
                    if (row) {
                        row.remove();
                    } else {
                        console.error("Row not found.");
                    }
                } else {
                    alert('Failed to delete category!');
                }
            })
            .catch(error => {
                alert('Error: ' + error);
            });
    }
}