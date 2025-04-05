import { Capacitor } from '@capacitor/core';
import { StatusBar, Style } from '@capacitor/status-bar';

export class Utilidades {
  static async cambiarStatusBar(style: 'Dark' | 'Light') {
    if (!Capacitor.isNativePlatform()) return;
    await StatusBar.setStyle({ style: Style[style] });
  }
}
