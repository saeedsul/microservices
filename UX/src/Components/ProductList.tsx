import { useEffect, useState } from 'react';
import { Product } from '../types';
import { useCart } from '../hooks/useCart';
import { handleRequest } from '../utils/requestWrapper';
import { getProducts } from '../apis/products/api';

const ProductList = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);
    const { addToCart } = useCart();

    useEffect(() => {
        const fetchProducts = async () => {
            const [response, err] = await handleRequest({ promise: getProducts() });
            if (err) {
                setError("Failed to load products");
            } else if (response) {
                setProducts(response?.data);
            }
            setLoading(false);
        };

        fetchProducts();
    }, []);

    if (loading) return <p>Loading...</p>;
    if (error) return <p style={{ color: 'red' }}>{error}</p>;
    if (products.length === 0) return <p>No products available.</p>;

    return (
             <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(250px, 1fr))', gap: '20px' }}>
                {products.map((item) => (
                    <div
                        key={item.id}
                        style={{
                            border: '1px solid #ccc',
                            borderRadius: '8px',
                            padding: '10px',
                            textAlign: 'center'
                        }}
                    >
                        
                        <h2 style={{ fontSize: '18px', marginTop: '10px' }}>{item.name}</h2>
                        <p style={{ fontSize: '14px', color: '#666' }}>  <strong>{item.name}</strong> - ${item.price}</p> 
                         <button onClick={() => addToCart(item)} style={{ marginLeft: '1rem' }}>
                        Add to Cart
                    </button>
                    </div>
                ))}
            </div> 
    );
};

export default ProductList;
