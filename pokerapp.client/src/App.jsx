import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Signup from '../components/Signup';
import Header from '../components/header';
import Footer from '../components/footer';
import Login from '../components/Login';
import Dashboard from '../components/Dashboard';
import Game from '../components/Game';
import { AuthProvider } from '../components/AuthContext';

function App() {
    return (
        <AuthProvider>
        <Router>
                <div className="App">
            <Header />
                <Routes>
                    <Route path="/" element={<Signup />} />
                    <Route path="/login" element={<Login />} />
                        <Route path="/dashboard" element={<Dashboard />} />
                    <Route path="/game/:gameId" element={<Game />} />
                    </Routes>
                    <Footer />
            </div>
            </Router>
        </AuthProvider>
    );
}

export default App;
