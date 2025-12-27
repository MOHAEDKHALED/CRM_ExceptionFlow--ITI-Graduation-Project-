# متطلبات تشغيل الفرونت إند (Frontend) في VSCode

## المتطلبات الأساسية

### 1. البرامج المطلوبة
- **Visual Studio Code (VSCode)**
  - تحميل من: https://code.visualstudio.com/
  - الإضافات الموصى بها:
    - ✅ Angular Language Service
    - ✅ ESLint
    - ✅ Prettier
    - ✅ TypeScript and JavaScript Language Features

- **Node.js** (الإصدار 18.x أو أحدث)
  - تحميل من: https://nodejs.org/
  - تحقق من التثبيت:
    ```cmd
    node --version
    npm --version
    ```
  - يجب أن يكون Node.js 18.x أو أحدث

- **Angular CLI** (اختياري لكن موصى به)
  ```cmd
  npm install -g @angular/cli
  ```
  - تحقق من التثبيت:
    ```cmd
    ng version
    ```

### 2. إعداد المشروع

#### خطوة 1: فتح المشروع في VSCode
1. افتح VSCode
2. اختر `File` → `Open Folder`
3. اذهب إلى مجلد: `frontend` (أو `CRM_ExceptionFlow\CRM_ExceptionFlow\crm-exceptionflow-ui`)
4. اضغط `Select Folder`

#### خطوة 2: تثبيت Dependencies
1. افتح Terminal في VSCode: `Terminal` → `New Terminal` أو `Ctrl + `` (Backtick)
2. نفذ الأمر:
```cmd
npm install
```
3. انتظر حتى يكتمل التثبيت (قد يستغرق بضع دقائق)

#### خطوة 3: تحديث API URL
1. افتح ملف: `src\environments\environment.ts`
2. عدل `apiUrl` ليشير إلى الباك إند:
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'  // أو https://localhost:5001/api
};
```

3. إذا كان لديك ملف `environment.prod.ts`، عدله أيضاً:
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-production-api-url.com/api'
};
```

### 3. تشغيل المشروع

#### الطريقة الأولى: استخدام npm
1. في Terminal في VSCode:
```cmd
npm start
```
أو
```cmd
ng serve
```

2. المتصفح سيفتح تلقائياً على: `http://localhost:4200`

#### الطريقة الثانية: استخدام Angular CLI
```cmd
ng serve --open
```

### 4. أوامر مفيدة

#### التطوير
```cmd
# تشغيل المشروع
npm start

# تشغيل مع فتح المتصفح تلقائياً
ng serve --open

# تشغيل على منفذ مختلف
ng serve --port 4201
```

#### البناء
```cmd
# بناء للإنتاج
npm run build

# بناء مع التحسينات
ng build --configuration production
```

#### الاختبار
```cmd
# تشغيل الاختبارات
npm test

# تشغيل الاختبارات مع التغطية
ng test --code-coverage
```

### 5. هيكل المشروع
```
frontend/
├── src/
│   ├── app/              # الكود الرئيسي للتطبيق
│   │   ├── core/         # الخدمات الأساسية والـ Guards
│   │   ├── features/     # المكونات الرئيسية (Auth, Dashboard, etc.)
│   │   └── shared/       # المكونات المشتركة
│   ├── environments/     # إعدادات البيئة
│   └── styles.scss       # الأنماط العامة
├── angular.json          # إعدادات Angular
├── package.json          # Dependencies
└── tsconfig.json         # إعدادات TypeScript
```

## استكشاف الأخطاء

### خطأ: "Cannot find module"
```cmd
# احذف node_modules و package-lock.json
rm -rf node_modules package-lock.json

# أعد التثبيت
npm install
```

### خطأ: "Port 4200 already in use"
```cmd
# استخدم منفذ آخر
ng serve --port 4201
```

### خطأ: "Angular CLI not found"
```cmd
# ثبت Angular CLI محلياً
npm install @angular/cli --save-dev

# أو عالمياً
npm install -g @angular/cli
```

### خطأ: CORS في المتصفح
- تأكد من أن الباك إند يعمل
- تحقق من `apiUrl` في `environment.ts`
- تأكد من إعدادات CORS في الباك إند

### خطأ: "Module not found"
```cmd
# نظف الكاش
npm cache clean --force

# أعد تثبيت Dependencies
rm -rf node_modules
npm install
```

## نصائح للتطوير

### 1. استخدام Extensions في VSCode
- **Angular Language Service**: دعم أفضل لـ Angular
- **ESLint**: للتحقق من جودة الكود
- **Prettier**: لتنسيق الكود تلقائياً
- **Auto Rename Tag**: لتعديل HTML tags تلقائياً
- **Bracket Pair Colorizer**: لتلوين الأقواس

### 2. إعدادات VSCode الموصى بها
أنشئ ملف `.vscode/settings.json`:
```json
{
  "editor.formatOnSave": true,
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "typescript.preferences.importModuleSpecifier": "relative",
  "files.exclude": {
    "**/node_modules": true,
    "**/.angular": true
  }
}
```

### 3. Debugging في VSCode
أنشئ ملف `.vscode/launch.json`:
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "chrome",
      "request": "launch",
      "name": "Launch Chrome",
      "url": "http://localhost:4200",
      "webRoot": "${workspaceFolder}"
    }
  ]
}
```

## ملاحظات مهمة
- ✅ تأكد من أن الباك إند يعمل قبل تشغيل الفرونت إند
- ✅ استخدم `npm install` بعد أي تغيير في `package.json`
- ✅ في حالة وجود أخطاء، امسح `node_modules` وأعد التثبيت
- ✅ استخدم `ng serve` للتطوير و `ng build` للإنتاج

