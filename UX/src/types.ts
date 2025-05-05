 
export interface Product {
    id: number;
    name: string;
    price: number;
}

export interface CartItem extends Product {
    quantity: number;
}

export interface CreateOrderResponse {
    status: string,
    message: string
}

 
export interface CreateOrderRequest {
    items: CartItem[];
    totalAmount: number; 
    customerId: number;
    shippingAddress: ShippingAddress
}

interface ShippingAddress {
    street: string,
    city: string,
    postalCode: string
}

export interface ApiResponse<T> {
    success: boolean,
    message: string,
    data: T
} 
 