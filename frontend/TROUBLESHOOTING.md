# حل المشاكل الشائعة - Frontend

## ✅ تم إصلاح: "Cannot find package 'restore-cursor'"

تم تثبيت الحزمة المفقودة. إذا ظهرت المشكلة مرة أخرى:

```bash
npm install restore-cursor --save-dev
```

## مشاكل أخرى شائعة

### 1. مشاكل في node_modules
```bash
# احذف node_modules وأعد التثبيت
rm -rf node_modules package-lock.json
npm install
```

### 2. مشاكل في Angular CLI
```bash
# ثبت Angular CLI محلياً
npm install @angular/cli --save-dev

# أو عالمياً
npm install -g @angular/cli@latest
```

### 3. مشاكل في Port
```bash
# استخدم منفذ آخر
ng serve --port 4201
```

### 4. مشاكل في البناء
```bash
# نظف الكاش
npm cache clean --force
rm -rf .angular
npm install
```

## الأمان (Security Vulnerabilities)

التحذيرات الأمنية في Angular 20 هي معروفة وستتم معالجتها في التحديثات القادمة. 
للتطوير المحلي، هذه التحذيرات آمنة نسبياً.

