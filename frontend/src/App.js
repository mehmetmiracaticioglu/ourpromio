import { BrowserRouter, Routes, Route } from 'react-router-dom';
import LoginForm from './components/LoginForm';
import RequireAuth from './components/RequireAuth';

function Home() {
  return <h2>Hoşgeldiniz! (Giriş yapıldı)</h2>;
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginForm />} />
        <Route path="/" element={
          <RequireAuth>
            <Home />
          </RequireAuth>
        } />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
