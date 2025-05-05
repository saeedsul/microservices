import axios from '../../utils/axiosInstance';
import { ApiResponse, Product } from '../../types';
 
 
export const getProducts = async (): Promise<ApiResponse<Product[]>> => {
    const response = await axios.get("/product");
    return response.data;
}