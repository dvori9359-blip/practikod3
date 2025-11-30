# אפליקציית TodoList - Fullstack

אפליקציה מלאה לניהול משימות (To-Do List) עם .NET Minimal API בצד השרת, React בצד הקליינט, ו-MySQL כמסד נתונים.

## מבנה הפרויקט

```
TodoApi/
├── Program.cs              # Minimal API - כל הקוד של השרת
├── appsettings.json        # הגדרות כולל Connection String
├── TodoApi.csproj          # קובץ הפרויקט וחבילות NuGet
└── ToDoListReact/          # אפליקציית React
    └── ToDoListReact-master/
        └── src/
            ├── App.js          # קומפוננט ראשי
            ├── service.js      # שירותי API
            └── index.js
```

## טכנולוגיות

### Server
- **.NET 8.0** - Minimal API
- **Entity Framework Core** - ORM לגישה למסד נתונים
- **Pomelo.EntityFrameworkCore.MySql** - ספק MySQL
- **Swashbuckle** - תיעוד Swagger
- **MySQL** - מסד נתונים

### Client
- **React 18.2**
- **Axios** - קריאות HTTP
- **React Scripts** - כלי בנייה

## התקנה והרצה

### דרישות מוקדמות
1. .NET 8.0 SDK
2. Node.js + npm
3. MySQL Server + MySQL Workbench
4. Visual Studio Code (מומלץ)

### הכנת מסד הנתונים

1. פתח MySQL Workbench והתחבר לשרת המקומי
2. צור Database חדש בשם `ToDoDB`:
```sql
CREATE DATABASE ToDoDB;
```

3. צור טבלת `Items`:
```sql
USE ToDoDB;

CREATE TABLE Items (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    IsComplete TINYINT(1)
);
```

### הגדרת השרת (.NET API)

1. פתח את התיקייה בטרמינל:
```bash
cd TodoApi
```

2. עדכן את ה-Connection String בקובץ `appsettings.json`:
```json
"ConnectionStrings": {
  "ToDoDB": "Server=localhost;Database=ToDoDB;User=root;Password=YOUR_PASSWORD"
}
```
**החלף את `YOUR_PASSWORD` בסיסמה שלך**

3. בנה והרץ את הפרויקט:
```bash
dotnet build
dotnet watch run
```

השרת יעלה על: `http://localhost:5000`

Swagger UI זמין ב: `http://localhost:5000/swagger`

### הגדרת הקליינט (React)

1. נווט לתיקיית React:
```bash
cd ToDoListReact/ToDoListReact-master
```

2. התקן תלויות:
```bash
npm install
```

3. הרץ את אפליקציית React:
```bash
npm start
```

האפליקציה תיפתח אוטומטית ב: `http://localhost:3000`

## API Endpoints

| Method | Endpoint | תיאור | Body |
|--------|----------|-------|------|
| GET | `/tasks` | שליפת כל המשימות | - |
| POST | `/tasks` | הוספת משימה חדשה | `{name: string, isComplete: bool}` |
| PUT | `/tasks/{id}` | עדכון משימה | `{id: int, name: string, isComplete: bool}` |
| DELETE | `/tasks/{id}` | מחיקת משימה | - |

## תכונות מיוחדות

### בצד השרת
- ✅ **CORS** - מוגדר לאפשר גישה מכל מקור
- ✅ **Swagger** - תיעוד אוטומטי של API
- ✅ **Minimal API** - קוד ממוזער ויעיל
- ✅ **Entity Framework** - גישה טיפוסית למסד נתונים

### בצד הקליינט
- ✅ **Axios Config Defaults** - כתובת API מוגדרת ברירת מחדל
- ✅ **Response Interceptor** - טיפול אוטומטי בשגיאות ורישום ללוג
- ✅ **React Hooks** - useState, useEffect
- ✅ **Async/Await** - קריאות אסינכרוניות נקיות

## Debugging ב-VS Code

1. לחץ על F5 או Run > Start Debugging
2. הגדר Breakpoints בקוד
3. השתמש ב-Watch ו-Call Stack לניפוי באגים

## פתרון בעיות

### השרת לא עולה
- ודא ש-MySQL Server פועל
- בדוק את ה-Connection String ב-`appsettings.json`
- הרץ `dotnet build` לבדיקת שגיאות קומפילציה

### הקליינט לא מתחבר לשרת
- ודא שהשרת רץ על `http://localhost:5000`
- בדוק את ה-Console בדפדפן לשגיאות CORS
- ודא ש-`axios.defaults.baseURL` ב-`service.js` מצביע נכון

### שגיאות במסד נתונים
- ודא שהטבלה `Items` קיימת במסד `ToDoDB`
- בדוק שהמשתמש והסיסמה נכונים ב-Connection String
- הרץ `dotnet ef database update` אם נדרש

## פיתוח נוסף (אתגרים)

### JWT Authentication
ניתן להוסיף מנגנון הזדהות:
1. טבלת Users במסד הנתונים
2. JWT Token Generation בשרת
3. דפי Login/Register בקליינט
4. Interceptor ל-401 שמעביר ל-Login

## רישיון
פרויקט לימודי - ללא רישיון ספציפי
