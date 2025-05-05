import { useState } from 'react';
import { useCart } from '../hooks/useCart';
import { handleRequest } from '../utils/requestWrapper';
import { create } from '../apis/cart/api';
import { CreateOrderRequest } from '../types';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const Cart = () => {
    const { items, updateQuantity, removeFromCart, totalAmount, clearCart } = useCart();

    const [showAddressForm, setShowAddressForm] = useState(false);
    const [shippingAddress, setShippingAddress] = useState({
        street: '',
        city: '',
        postalCode: ''
    });

    const formatPrice = (amount: number): string => (Math.round(amount * 100) / 100).toFixed(2);
    const formattedTotal = formatPrice(totalAmount);

    const createOrderObject = (): CreateOrderRequest => {
        const orderItems = items.map(item => ({
            id: item.id,
            name: item.name,
            price: item.price,
            quantity: item.quantity
        }));

        return {
            customerId: 123,
            items: orderItems,
            totalAmount,
            shippingAddress
        };
    };

    const handlePlaceOrder = async () => {
        if (!shippingAddress.street || !shippingAddress.city || !shippingAddress.postalCode) {
            toast.error("Please fill in all shipping address fields.");
            return;
        }

        const order = createOrderObject(); console.log(JSON.stringify(order));
        const [response, error] = await handleRequest({ promise: create(order) });
        
        if (error) {
            toast.error('Failed to place order.');
        } else {
            if (response?.data.status === "Pending") {
                toast.success(response?.data.message);
            }
            else {
                toast.error(response?.data.message);
            }
            clearCart();
            setShowAddressForm(false);
            setShippingAddress({ street: '', city: '', postalCode: '' });
        }
    };

    return (
        <div> 

            <h5>
                <i className="bi bi-cart me-2" style={{ fontSize: '1.5rem' }}></i>
                Shopping Cart
            </h5>

            {items.length === 0 && <p className="text-muted">Cart is empty.</p>}

            <div className="list-group mb-3">
                {items.map(item => (
                    <div className="list-group-item d-flex justify-content-between align-items-center" key={item.id}>
                        <div className="me-3">
                            <div><strong>{item.name}</strong></div>
                            <div className="text-muted">&#163;{item.price} each</div>
                        </div>

                        <div className="d-flex align-items-center">
                            <input
                                type="number"
                                className="form-control form-control-sm"
                                style={{ width: '60px' }}
                                value={item.quantity}
                                min={1}
                                onChange={(e) => updateQuantity(item.id, parseInt(e.target.value))}
                            />
                            <span className="mx-2">
                                = &#163;{formatPrice(item.price * item.quantity)}
                            </span>

                            <i
                                className="bi bi-trash text-danger"
                                style={{ cursor: 'pointer' }}
                                onClick={() => removeFromCart(item.id)}
                                title="Remove item"
                            ></i>
                        </div>
                    </div>
                ))}
            </div>

            <h5>Total: &#163;{formattedTotal}</h5> 

            {items.length > 0 && !showAddressForm && (
                <button
                    className="btn btn-primary mt-3"
                    onClick={() => setShowAddressForm(true)}
                >
                    Checkout
                </button>

            )}

            {showAddressForm && (
                <div className="mt-3">
                    <h6>Shipping Address</h6>
                    <div className="mb-2">
                        <input
                            type="text"
                            className="form-control"
                            placeholder="Street"
                            value={shippingAddress.street}
                            onChange={(e) => setShippingAddress({ ...shippingAddress, street: e.target.value })}
                        />
                    </div>
                    <div className="mb-2">
                        <input
                            type="text"
                            className="form-control"
                            placeholder="City"
                            value={shippingAddress.city}
                            onChange={(e) => setShippingAddress({ ...shippingAddress, city: e.target.value })}
                        />
                    </div>
                    <div className="mb-2">
                        <input
                            type="text"
                            className="form-control"
                            placeholder="Postal Code"
                            value={shippingAddress.postalCode}
                            onChange={(e) => setShippingAddress({ ...shippingAddress, postalCode: e.target.value })}
                        />
                    </div>
                    <button className="btn btn-success" onClick={handlePlaceOrder}>
                        Place Order
                    </button>
                </div>
            )}
        </div>
    );
};

export default Cart;
