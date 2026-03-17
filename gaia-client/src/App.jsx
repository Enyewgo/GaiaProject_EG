import { useState, useEffect } from "react";
import "./App.css";

function App() {
  // --- State Management ---
  const [num1, setNum1] = useState(0);
  const [num2, setNum2] = useState(0);
  const [mathAction, setMathAction] = useState("add");
  const [userName, setUserName] = useState("E.G.");
  const [textInput, setTextInput] = useState("");
  const [textAction, setTextAction] = useState("reverse");
  const [result, setResult] = useState(null);
  const [history, setHistory] = useState([]);

  // --- API Calls ---
  const loadHistory = async () => {
    try {
      const response = await fetch(
        "https://localhost:7235/api/Calculator/history",
      );
      const data = await response.json();
      setHistory(data);
    } catch (error) {
      console.error("נכשל בטעינת היסטוריה", error);
    }
  };

  useEffect(() => {
    loadHistory();
  }, []);

  const runMath = async () => {
    const response = await fetch(
      `https://localhost:7235/api/Calculator/calculate?n1=${num1}&n2=${num2}&action=${mathAction}&user=${userName}`,
    );
    const data = await response.json();
    setResult(data);
    loadHistory();
  };

  const runText = async () => {
    const response = await fetch(
      `https://localhost:7235/api/Calculator/string-action?input=${textInput}&action=${textAction}&user=${userName}`,
    );
    const data = await response.json();
    setResult(data);
    loadHistory();
  };

  return (
    <div className="container">
      <h1>פרויקט גאיה - E.G</h1>

      {/* תיבת משתמש דינמי */}
      <div
        className="card"
        style={{
          border: "1px solid #d4af37",
          marginBottom: "20px",
          padding: "10px",
        }}
      >
        <label style={{ fontWeight: "bold", color: "#d4af37" }}>
          משתמש פעיל:{" "}
        </label>
        <input
          type="text"
          value={userName}
          onChange={(e) => setUserName(e.target.value)}
          style={{
            marginRight: "10px",
            padding: "5px",
            borderRadius: "4px",
            border: "1px solid #ccc",
          }}
        />
      </div>

      <div className="card">
        <h3>מחשבון מספרים</h3>
        <input
          type="number"
          value={num1}
          onChange={(e) => setNum1(e.target.value)}
        />
        <input
          type="number"
          value={num2}
          onChange={(e) => setNum2(e.target.value)}
        />
        <select
          value={mathAction}
          onChange={(e) => setMathAction(e.target.value)}
        >
          <option value="add">חיבור (+)</option>
          <option value="sub">חיסור (-)</option>
          <option value="mul">כפל (*)</option>
          <option value="div">חילוק (/)</option>
        </select>
        <button className="gold-button" onClick={runMath}>
          בצע חישוב
        </button>
      </div>

      <div className="card">
        <h3>מעבד מחרוזות</h3>
        <input
          type="text"
          value={textInput}
          onChange={(e) => setTextInput(e.target.value)}
          placeholder="הקלד טקסט..."
        />
        <select
          value={textAction}
          onChange={(e) => setTextAction(e.target.value)}
        >
          <option value="reverse">היפוך (ידני)</option>
          <option value="uppercase">הגדלה (ASCII)</option>
          <option value="length">חישוב אורך</option>
        </select>
        <button className="gold-button" onClick={runText}>
          עבד טקסט
        </button>
      </div>

      {/* תצוגת תוצאה נוכחית עם בונוסים */}
      {result && (
        <div
          className="result-area"
          style={{
            marginTop: "20px",
            padding: "15px",
            border: "2px solid #d4af37",
            borderRadius: "10px",
            backgroundColor: "#1a1a1a",
          }}
        >
          <h4 style={{ color: "#d4af37" }}>תוצאה: {result.result}</h4>
          <p>
            בוצע עבור פרויקט גאיה ע''י: <strong>{result.user}</strong>
          </p>

          <div
            style={{
              fontSize: "0.9em",
              borderTop: "1px solid #d4af37",
              marginTop: "10px",
              paddingTop: "10px",
              color: "#aaa",
            }}
          >
            <p>
              📈 פעולות <strong>{mathAction}</strong> החודש:{" "}
              <span style={{ color: "#d4af37" }}>{result.monthlyCount}</span>
            </p>
            {result.lastThree && result.lastThree.length > 0 && (
              <p>
                🕒 3 תוצאות אחרונות מסוג זה:{" "}
                <span style={{ color: "#d4af37" }}>
                  {result.lastThree.join(", ")}
                </span>
              </p>
            )}
          </div>
        </div>
      )}

      {/* טבלת היסטוריה */}
      <div style={{ marginTop: "30px" }}>
        <h3>היסטוריית חישובים מה-SQL</h3>
        <table
          border="1"
          style={{
            width: "100%",
            textAlign: "center",
            borderColor: "#d4af37",
            color: "white",
          }}
        >
          <thead>
            <tr style={{ backgroundColor: "#333", color: "#d4af37" }}>
              <th>סוג</th>
              <th>פעולה</th>
              <th>קלט</th>
              <th>תוצאה</th>
              <th>בוצע ע"י</th>
            </tr>
          </thead>
          <tbody>
            {history.map((item) => (
              <tr key={item.id}>
                <td>{item.operationType}</td>
                <td>{item.actionName}</td>
                <td>{item.inputData}</td>
                <td>{item.resultValue}</td>
                <td style={{ fontWeight: "bold", color: "#d4af37" }}>
                  {item.performedBy}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
} // סגירת פונקציית App

export default App;
