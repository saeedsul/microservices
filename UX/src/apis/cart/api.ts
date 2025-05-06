import axios from '../../utils/axiosInstance';
import { ApiResponse, CreateOrderRequest, CreateOrderResponse } from '../../types';
 
 
 
export const create = async (order: CreateOrderRequest): Promise<ApiResponse<CreateOrderResponse>> => {
    const response = await axios.post("/api/order/create", order);
    return response.data;
};