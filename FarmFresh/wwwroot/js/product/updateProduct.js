async function productUpdate(event) {
    event.preventDefault();

    const form = document.getElementById("editForm");
    const formData = new FormData(form);
    const productId = document.getElementById("ProductId")?.value;
    const baseUrl = window.location.origin;

    if (!productId) {
        console.error('Product ID is missing.');
        alert('Failed to retrieve the Product ID.');
        return false;
    }

    try {
        console.log('API Endpoint:', `${baseUrl}/api/products/edit/${productId}`);
        const response = await fetch(`${baseUrl}/api/products/edit/${productId}`, {
            method: 'PUT',
            body: formData,
        });

        if (!response.ok) {
            console.error('Response Error:', response.status);
            throw new Error('Failed to update product.');
        }

        const data = await response.json();

        if (data.success) {
            alert('Product updated successfully!');
            window.location.href = '/api/products/allproducts';
        } else {
            console.error('API Error:', data);
            alert('Failed to update product. Please try again.');
        }

    } catch (e) {
        console.error('Error updating product:', e);
        alert('An error occurred while updating the product.');
    }

    return false;
}
