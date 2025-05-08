# Blazor WebAssembly Learning Application

This is a sample Blazor WebAssembly (WASM) application designed for learning purposes. It demonstrates key concepts of Blazor WebAssembly development including:

- Multi-page navigation with a tabbed interface
- Authentication and authorization flows
- Dynamic data loading based on dropdown selection
- Loading states and indicators
- Service-based architecture

## Features

- **Authentication System**: Simple login/logout functionality that controls access to pages
- **Dynamic Data Loading**: Data refreshes when category dropdown selection changes
- **Loading States**: Visual indicators for all async operations
- **Responsive Design**: Works on various screen sizes
- **State Management**: Using services to maintain application state
- **Chart Visualization**: Using Chart.js for data visualization
- **Form Validation**: Input validation on forms

## Pages and Components

1. **Dashboard**: Overview with key metrics and recent activity
2. **Products**: List of products filtered by selected category
3. **Users**: User management interface with modal details
4. **Reports**: Data visualization with charts and export options
5. **Settings**: User and category configuration options
6. **Login**: Authentication page

## Architecture

The application follows a service-based architecture:

- **AppStateService**: Manages global application state
- **AuthService**: Handles authentication operations
- **DataService**: Provides data for all components
- **ChartJsInterop**: JavaScript interop for Chart.js integration

## Key Concepts Demonstrated

### State Management

- Using a centralized service for application state
- Event-based notification system for state changes

### Component Lifecycle

- OnInitializedAsync for initial data loading
- OnAfterRenderAsync for JavaScript interoperability
- IDisposable implementation for cleanup

### Loading States

- Global loading indicator in MainLayout
- Component-specific loading states
- Simulated API delays for realistic behavior

### Navigation

- Programmatic navigation
- Route parameters
- Navigation guard for unauthenticated users

### JavaScript Interop

- Chart.js integration
- Custom JavaScript methods

## Getting Started

1. Make sure you have the .NET 9 SDK installed
2. Clone the repository
3. Run the application with `dotnet run`
4. Navigate to the URL provided in the console
5. Login with any username and password

## Extending the Application

Here are some ways you can extend this application for further learning:

1. Add a registration page
2. Implement real API calls to a backend service
3. Add form validation with FluentValidation
4. Implement real-time updates with SignalR
5. Add unit tests with bUnit
6. Implement role-based authorization
7. Add a theme switcher using CSS variables

## License

This project is licensed under the MIT License - see the LICENSE file for details.