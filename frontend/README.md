# Frontend - CRM Exception Flow

Angular 20 Frontend Application with modern design and animations.

## Getting Started

### Prerequisites
- Node.js 18.x or later
- npm or yarn

### Installation

```bash
npm install
```

### Development

```bash
npm start
```

The app will be available at `http://localhost:4200`

### Build

```bash
npm run build
```

## Project Structure

```
src/
├── app/
│   ├── core/           # Core services, guards, interceptors
│   ├── features/       # Feature modules (auth, dashboard, etc.)
│   └── shared/         # Shared components
├── environments/       # Environment configurations
└── styles.scss        # Global styles
```

## Configuration

Update `src/environments/environment.ts` with your API URL:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

## Features

- ✅ Modern Angular 20 with standalone components
- ✅ Beautiful UI with animations
- ✅ JWT Authentication
- ✅ Role-based access control
- ✅ Responsive design
- ✅ Toast notifications

