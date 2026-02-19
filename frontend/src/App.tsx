import { useState, useEffect } from 'react'

function App() {
    const [telegramId, setTelegramId] = useState<number | null>(null);
    const [code, setCode] = useState("")
    
    const API_URL = "http://localhost:5189/api";

    useEffect(() => {
        checkAuth();
    }, []);
    
    const checkAuth = async () => {
        const response = await fetch(`${API_URL}/auth/me`, {
            method: 'GET',
            credentials: 'include'
        });

        if (response.ok) {
            const data = await response.json();
            setTelegramId(data.id);
        }
    }

    const handleLogin = async () => {
        try {
            const response = await fetch(`${API_URL}/auth/verify`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ code: code }),

                credentials: 'include'
            });

            if (response.ok) {
                checkAuth();
                setCode("");
            } else {
                alert("неверный код");
            }
        } catch (e) {
            console.error(e);
        }
    }
    
    const handleLogout = async () => {
        setTelegramId(null);
    }
    
    if(telegramId) {
        return(
        <div style={{textAlign: 'center', marginTop: 50}}>
            <h1>✅ Вы авторизованы!</h1>
            <h2>Ваш Telegram ID: <span style={{color: 'green'}}>{telegramId}</span></h2>

            <br/>
            <button onClick={handleLogout} style={{
                padding: '10px 20px',
                backgroundColor: '#ff4d4d',
                color: 'white',
                border: 'none',
                borderRadius: 5,
                cursor: 'pointer'
            }}>
                Выйти
            </button>

            {/* todo */}
        </div>
        );
    }

    return (
        <div style={{padding: 50, textAlign: 'center'}}>
            <h1>🏎 CarTracker Web</h1>

            <div style={{border: '1px solid #ccc', padding: 20, borderRadius: 10}}>

                <p>Введите код из Телеграм бота (/login):</p>
                <input
                    type="text"
                    value={code}
                    onChange={e => setCode(e.target.value)}
                    placeholder="123456"
                    style={{ fontSize: 20, padding: 5 }}
                />
                <br/><br/>
                <button onClick={handleLogin} style={{ padding: '10px 20px', cursor: 'pointer' }}>
                    Войти
                </button>
            </div>
        </div>
    )
}

export default App
