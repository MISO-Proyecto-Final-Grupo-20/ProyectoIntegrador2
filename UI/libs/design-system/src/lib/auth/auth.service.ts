/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { Sesion } from '../modelos/generales.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private tokenKey = 'store_token';

  constructor() {
    const token = this.obtenerToken();
    if (token) {
      const decoded = jwtDecode(token);
      if (decoded) {
        (window as any).sesion = decoded;
      }
    }
  }

  registrarToken(token: string): void {
    const tokenAEnviar = `Bearer ${token}`;
    localStorage.setItem(this.tokenKey, tokenAEnviar);

    const decoded = jwtDecode(tokenAEnviar);
    if (decoded) {
      (window as any).sesion = decoded;
    }
  }

  obtenerDatosSesion(): Sesion | null {
    const token = this.obtenerToken();
    if (token) {
      const decoded = jwtDecode(token) as Sesion;
      if (decoded) {
        (window as any).sesion = decoded;
        return decoded;
      }
    }
    return null;
  }

  obtenerToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  eliminarToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  tieneToken(): boolean {
    return !!this.obtenerToken();
  }
}
