import { useState, ReactNode } from 'react';
import { CartItem, Product } from '../types';
import { CartContext } from './CartContext';

export const CartProvider = ({ children }: { children: ReactNode }) => {
    const [items, setItems] = useState<CartItem[]>([]);

    const addToCart = (product: Product, quantity: number = 1) => {
        setItems(prev => {
            const existing = prev.find(i => i.id === product.id);
            if (existing) {
                return prev.map(i =>
                    i.id === product.id ? { ...i, quantity: i.quantity + quantity } : i
                );
            }
            return [...prev, { ...product, quantity }];
        });
    };

    const removeFromCart = (productId: number) => {
        setItems(prev => prev.filter(i => i.id !== productId));
    };

    const updateQuantity = (productId: number, quantity: number) => {
        setItems(prev =>
            prev.map(i =>
                i.id === productId ? { ...i, quantity } : i
            )
        );
    };

    const clearCart = () => setItems([]);

    const totalAmount = items.reduce((sum, i) => sum + i.price * i.quantity, 0);

    return (
        <CartContext.Provider value={{ items, addToCart, removeFromCart, updateQuantity, clearCart, totalAmount }}>
            {children}
        </CartContext.Provider>
    );
};
