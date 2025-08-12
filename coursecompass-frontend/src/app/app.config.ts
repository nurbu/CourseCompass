import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { initializeApp, provideFirebaseApp } from '@angular/fire/app';
import { getAuth, provideAuth } from '@angular/fire/auth';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideAnimationsAsync(),
    provideHttpClient(withInterceptorsFromDi()), provideFirebaseApp(() => initializeApp({"projectId":"coursecompass-78332","appId":"1:40528551677:web:608bd06c8ee56403672894","storageBucket":"coursecompass-78332.firebasestorage.app","apiKey":"AIzaSyC1e7HzA-u5yWyZ6qXKiWKzIeagc9tDhm4","authDomain":"coursecompass-78332.firebaseapp.com","messagingSenderId":"40528551677","measurementId":"G-FVHEQ22NQ7"})), provideAuth(() => getAuth())
  ]
};
