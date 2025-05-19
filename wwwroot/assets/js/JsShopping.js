$(document).ready(function () {
    ShowCount();

    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        console.log("Nút .btnAddToCart được click");

        var id = $(this).data('id');
        var quantity = 1;
        var tQuantity = $('#quantity_value').text().trim();
        if (tQuantity !== '') {
            quantity = parseInt(tQuantity);
        }

        console.log("ID:", id, "Quantity:", quantity);

        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                console.log("Phản hồi từ server:", rs);
                if (rs.success) {
                    $('#checkout_items').html(rs.count);
                    alert(rs.message);
                    ShowCount();
                } else {
                    alert("Thêm thất bại: " + (rs.message || "Lỗi không xác định"));
                }
            },
            error: function (xhr, status, error) {
                console.error("Lỗi AJAX:", status, error);
                console.error("Phản hồi từ server:", xhr.responseText);
                alert('Đã xảy ra lỗi khi thêm vào giỏ hàng. Vui lòng thử lại.');
            }
        });
    });
});

function ShowCount() {
    $.ajax({
        url: '/shoppingcart/showCount',
        type: 'GET',
        data: {},
        success: function (rs) {
            console.log("Phản hồi từ server:", rs);
            $('#checkout_items').html(rs.count);
        },
        error: function (xhr, status, error) {
            console.error("Lỗi AJAX:", status, error);
            console.error("Phản hồi từ server:", xhr.responseText);
            alert('Không thể tải số lượng giỏ hàng. Vui lòng thử lại.');
        }
    });
}
$(document).ready(function () {
    console.log("JavaScript đã tải và DOM sẵn sàng");

    // Xóa từng sản phẩm
    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();

        if (!confirm("Bạn có chắc chắn muốn xóa sản phẩm này không?")) {
            return;
        }

        var id = $(this).data('id');
        if (!id || isNaN(parseInt(id))) {
            alert("ID sản phẩm không hợp lệ");
            return;
        }
        id = parseInt(id);

        $.ajax({
            url: '/shoppingcart/Delete',
            type: 'POST',
            data: { id: id },
            success: function (rs) {
                if (rs.success) {
                    $('#checkout_items').html(+ rs.count);
                    $('#trow' + id).remove();

                    if (rs.count === 0) {
                        $('.table tbody').html('<tr><td colspan="8">Giỏ hàng trống.</td></tr>');
                        $('#total-price').text('0 VNĐ');
                    } else {
                        // Tính lại tổng tiền từ các hàng còn lại
                        let total = 0;
                        $('.total-price').each(function () {
                            let price = parseFloat($(this).text().replace(' VNĐ', '').replace(/,/g, ''));
                            if (!isNaN(price)) total += price;
                        });
                        $('#total-price').text(total.toLocaleString('vi-VN') + ' VNĐ');
                    }
                } else {
                    alert(rs.message || "Xóa thất bại");
                }
            },
            error: function (xhr, status, error) {
                alert('Đã xảy ra lỗi khi xóa sản phẩm. Status: ' + status);
            }
        });
    });

    // Xóa toàn bộ giỏ hàng
    $(document).on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();

        if (confirm('Bạn có chắc muốn xóa toàn bộ giỏ hàng?')) {
            $.ajax({
                url: '/shoppingcart/DeleteAll',
                type: 'POST',
                success: function (rs) {
                    if (rs.success) {
                        $('.table tbody').html(`
                            <tr>
                                <td colspan="8" class="text-center">Giỏ hàng trống.</td>
                            </tr>
                        `);
                        $('#total-price').text("0 VNĐ");
                        $('#checkout_items').html('0');
                    } else {
                        alert(rs.message);
                    }
                }
            });
        }
    });
});



function DeleteAll() {
    $.ajax({
        url: '/shoppingcart/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.success) {
                alert(rs.message);
                LoadCart(); // Load lại phần giỏ hàng
            } else {
                alert(rs.message);
            }
        }
    });
}
function LoadCart() {
    $.ajax({
        url: '/shoppingcart/Partial_Item_Cart',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs);
        }
    });
}
$(document).ready(function () {
    // Sử dụng event delegation nếu button được tạo động
    $(document).on('click', '.btnUpdate', function (e) {
        e.preventDefault();
        console.log('Button clicked'); // Kiểm tra event trigger

        var id = $(this).data('id');
        var quantity = $('#Quantity_' + id).val();
        console.log('ID:', id, 'Quantity:', quantity); // Kiểm tra giá trị

        $.ajax({
            url: '/ShoppingCart/Update', // Đảm bảo đúng route
            type: 'POST',
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                console.log('Response:', res); // Kiểm tra response
                if (res.success) {
                    $('#trow_' + id + ' .total-price').text(res.itemTotal + ' VNĐ');
                    $('#total-price').text(res.total + ' VNĐ');
                    $('#checkout_items').text(res.count);
                } else {
                    alert(res.message);
                }
            },
            error: function (xhr, status, error) {
                console.error('AJAX Error:', status, error); // Log lỗi
            }
        });
    });
});