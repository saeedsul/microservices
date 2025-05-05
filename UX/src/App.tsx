import { CartProvider } from './context/CartProvider';
import ProductList from './Components/ProductList';
import Cart from './Components/Cart';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const App = () => {
    return (
        <CartProvider>
            <div className="container my-4">
                <h1 className="mb-4">Electronic Store</h1>
                <div className="row">
                    <div className="col-md-8">
                        <ProductList />
                    </div>
                    <div className="col-md-4">
                        <Cart />
                    </div>
                </div>
            </div>
             
            <ToastContainer position="top-right" autoClose={3000} style={{ zIndex: 9999 }} />
        </CartProvider>
    );
};

export default App;
