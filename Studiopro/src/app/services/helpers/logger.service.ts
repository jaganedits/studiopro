import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';

/**
 * Log levels enum
 */
export enum LogLevel {
  Debug = 0,
  Info = 1,
  Warn = 2,
  Error = 3,
  None = 4,
}

/**
 * Modern Logger Service
 * - Environment-aware logging
 * - Configurable log levels
 * - Structured logging support
 * - Color-coded console output
 */
@Injectable({
  providedIn: 'root',
})
export class LoggerService {
  private readonly logLevel: LogLevel = environment.production ? LogLevel.Error : LogLevel.Debug;

  /**
   * Debug level logging
   */
  debug(message: string, ...args: unknown[]): void {
    this.writeLog(LogLevel.Debug, message, args);
  }

  /**
   * Info level logging
   */
  info(message: string, ...args: unknown[]): void {
    this.writeLog(LogLevel.Info, message, args);
  }

  /**
   * Log alias for backward compatibility
   */
  log(message: string, style?: string, ...args: unknown[]): void {
    if (this.shouldLog(LogLevel.Info)) {
      if (style) {
        console.log(`%c${message}`, style, ...args);
      } else {
        console.log(message, ...args);
      }
    }
  }

  /**
   * Warning level logging
   */
  warn(message: string, ...args: unknown[]): void {
    this.writeLog(LogLevel.Warn, message, args);
  }

  /**
   * Error level logging
   */
  error(message: string, ...args: unknown[]): void {
    this.writeLog(LogLevel.Error, message, args);
  }

  /**
   * Log a table (useful for arrays/objects)
   */
  table(data: unknown, columns?: string[]): void {
    if (this.shouldLog(LogLevel.Debug)) {
      if (columns) {
        console.table(data, columns);
      } else {
        console.table(data);
      }
    }
  }

  /**
   * Group related logs
   */
  group(label: string): void {
    if (this.shouldLog(LogLevel.Debug)) {
      console.group(label);
    }
  }

  /**
   * End log group
   */
  groupEnd(): void {
    if (this.shouldLog(LogLevel.Debug)) {
      console.groupEnd();
    }
  }

  /**
   * Time a block of code
   */
  time(label: string): void {
    if (this.shouldLog(LogLevel.Debug)) {
      console.time(label);
    }
  }

  /**
   * End timer and log duration
   */
  timeEnd(label: string): void {
    if (this.shouldLog(LogLevel.Debug)) {
      console.timeEnd(label);
    }
  }

  /**
   * Check if current log level should be logged
   */
  private shouldLog(level: LogLevel): boolean {
    return level >= this.logLevel && !environment.production;
  }

  /**
   * Write log with formatting
   */
  private writeLog(level: LogLevel, message: string, args: unknown[]): void {
    if (!this.shouldLog(level)) return;

    const timestamp = new Date().toISOString();
    const levelColors: Record<LogLevel, string> = {
      [LogLevel.Debug]: 'color: gray',
      [LogLevel.Info]: 'color: blue',
      [LogLevel.Warn]: 'color: orange',
      [LogLevel.Error]: 'color: red',
      [LogLevel.None]: '',
    };

    const levelLabels: Record<LogLevel, string> = {
      [LogLevel.Debug]: 'DEBUG',
      [LogLevel.Info]: 'INFO',
      [LogLevel.Warn]: 'WARN',
      [LogLevel.Error]: 'ERROR',
      [LogLevel.None]: '',
    };

    const formattedMessage = `[${timestamp}] [${levelLabels[level]}] ${message}`;

    switch (level) {
      case LogLevel.Debug:
        console.debug(`%c${formattedMessage}`, levelColors[level], ...args);
        break;
      case LogLevel.Info:
        console.info(`%c${formattedMessage}`, levelColors[level], ...args);
        break;
      case LogLevel.Warn:
        console.warn(`%c${formattedMessage}`, levelColors[level], ...args);
        break;
      case LogLevel.Error:
        console.error(`%c${formattedMessage}`, levelColors[level], ...args);
        break;
    }
  }
}