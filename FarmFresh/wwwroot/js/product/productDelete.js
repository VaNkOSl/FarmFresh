function deleteProduct() {
    if (confirm('Are you sure you want to delete this product?')) {
        var formData = new FormData(document.getElementById("deleteForm"));
        var id = formData.get("id");

        fetch(`/api/products/delete/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: id })
        })
            .then(response => {
                if (response.ok) {
                    alert('Product deleted successfully.');
                    window.location.href = '/api/products/allproducts';
                } else {
                    alert('Error deleting product. Plese try again later.');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Something went wrong.');
            });
    }
}
