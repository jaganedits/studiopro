import { Injectable, inject } from '@angular/core';
import { ErrorHandlerService } from './error-handler.service';
import type * as XLSX from 'xlsx';

/**
 * Export format types
 */
export type ExportFormat = 'pdf' | 'excel' | 'csv';

/**
 * Excel extension types
 */
export type ExcelExtension = 'xlsx' | 'csv';

/**
 * PDF page orientation
 */
export type PageOrientation = 'portrait' | 'landscape';

/**
 * Export options interface
 */
export interface ExportOptions {
  filename: string;
  format: ExportFormat;
  excelExtension?: ExcelExtension;
  orientation?: PageOrientation;
  title?: string;
  subtitle?: string;
  headerStyle?: {
    fillColor?: [number, number, number];
    textColor?: number;
    fontSize?: number;
  };
  /** Use styled Excel with colored headers and title row */
  styled?: boolean;
  /** Header background color in hex (e.g., '7C3AED' for purple) */
  headerColor?: string;
}

/**
 * Export data type - generic record with string/number values
 */
export type ExportData = Record<string, string | number | boolean | null | undefined>;

/**
 * Modern Export Service
 * - Supports PDF, Excel, CSV exports
 * - Dynamic imports for lazy loading
 * - Customizable styling options
 */
@Injectable({
  providedIn: 'root',
})
export class ExportService {
  private readonly errorHandler = inject(ErrorHandlerService);

  /**
   * Export data to specified format
   */
  async export(data: ExportData[], options: ExportOptions): Promise<void> {
    if (!data || data.length === 0) {
      this.errorHandler.showWarning('No Data', 'There is no data to export.');
      return;
    }

    try {
      switch (options.format) {
        case 'pdf':
          await this.exportToPdf(data, options);
          break;
        case 'excel':
          if (options.styled) {
            await this.exportToStyledExcel(data, options);
          } else {
            await this.exportToExcel(data, options.filename, options.excelExtension ?? 'xlsx');
          }
          break;
        case 'csv':
          await this.exportToExcel(data, options.filename, 'csv');
          break;
      }
    } catch (error) {
      console.error('Export failed:', error);
      this.errorHandler.showError('Export Failed', 'Failed to export data. Please try again.');
    }
  }

  /**
   * Quick export method (legacy compatibility)
   */
  exportFiles(exportType: string, fileName: string, dataList: ExportData[]): void {
    if (!dataList || dataList.length === 0) {
      this.errorHandler.showWarning('Warning Message', 'There is no data to download.');
      return;
    }

    const format = exportType === 'pdf' ? 'pdf' : 'excel';
    this.export(dataList, { filename: fileName, format });
  }

  // ==================== PDF EXPORT ====================

  /**
   * Export data to PDF with compact professional styling
   * Optimized to fit more records per page
   */
  private async exportToPdf(
    data: ExportData[],
    options: ExportOptions
  ): Promise<void> {
    const [{ default: jsPDF }, { default: autoTable }] = await Promise.all([
      import('jspdf'),
      import('jspdf-autotable'),
    ]);

    const filename = options.filename.endsWith('.pdf')
      ? options.filename
      : `${options.filename}.pdf`;

    const orientation = options.orientation ?? 'landscape';
    const doc = new jsPDF(orientation === 'landscape' ? 'l' : 'p', 'mm', 'a4');
    const pageWidth = doc.internal.pageSize.getWidth();

    // Parse header color from hex to RGB
    const headerColorHex = options.headerColor || '7C3AED';
    const headerRGB = this.hexToRgb(headerColorHex);

    // Compact title section
    const title = options.title || options.filename;
    const recordCount = data.length;
    const currentDate = new Date().toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });

    // Draw compact header bar
    doc.setFillColor(headerRGB.r, headerRGB.g, headerRGB.b);
    doc.rect(0, 0, pageWidth, 10, 'F');

    // Title on header bar
    doc.setFontSize(11);
    doc.setTextColor(255, 255, 255);
    doc.setFont('helvetica', 'bold');
    doc.text(title, 8, 7);

    // Record count and date on right side of header
    doc.setFontSize(8);
    doc.setFont('helvetica', 'normal');
    doc.text(`${recordCount} records | ${currentDate}`, pageWidth - 8, 7, { align: 'right' });

    // Prepare table data with # column
    const dataHeaders = Object.keys(data[0]);
    const headers = ['#', ...dataHeaders];
    const body = data.map((row, index) => [
      (index + 1).toString(),
      ...dataHeaders.map((header) => String(row[header] ?? ''))
    ]);

    // Column styles with # column
    const columnStyles: Record<number, any> = {
      0: { halign: 'center', cellWidth: 8 }, // # column - narrow and centered
      ...this.getColumnStyles(dataHeaders, 1) // Offset other columns by 1
    };

    // Generate compact styled table
    autoTable(doc, {
      head: [headers],
      body,
      startY: 12,
      theme: 'grid',
      styles: {
        fontSize: 8,
        cellPadding: 1.5,
        lineColor: [220, 220, 220],
        lineWidth: 0.1,
        font: 'helvetica',
        overflow: 'linebreak',
        minCellHeight: 5,
      },
      headStyles: {
        fillColor: [headerRGB.r, headerRGB.g, headerRGB.b],
        textColor: [255, 255, 255],
        fontSize: 8,
        fontStyle: 'bold',
        halign: 'center',
        cellPadding: 2,
        minCellHeight: 6,
      },
      bodyStyles: {
        textColor: [40, 40, 40],
        fillColor: [255, 255, 255],
      },
      alternateRowStyles: {
        fillColor: [248, 248, 248],
      },
      columnStyles,
      margin: { left: 8, right: 8, top: 12, bottom: 12 },
      didDrawPage: (data) => {
        // Compact footer
        const pageCount = doc.getNumberOfPages();
        const currentPage = data.pageNumber;
        const pageHeight = doc.internal.pageSize.getHeight();

        doc.setFontSize(7);
        doc.setTextColor(130, 130, 130);
        doc.text('Studio Pro', 8, pageHeight - 5);
        doc.text(`Page ${currentPage} of ${pageCount}`, pageWidth - 8, pageHeight - 5, { align: 'right' });
      },
    });

    // Save the PDF
    doc.save(filename);
  }

  /**
   * Convert hex color to RGB
   */
  private hexToRgb(hex: string): { r: number; g: number; b: number } {
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result
      ? {
          r: parseInt(result[1], 16),
          g: parseInt(result[2], 16),
          b: parseInt(result[3], 16),
        }
      : { r: 124, g: 58, b: 237 }; // Default purple
  }

  /**
   * Get column styles based on header names
   * @param headers - Array of header names
   * @param offset - Index offset for column positions (default 0)
   */
  private getColumnStyles(headers: string[], offset = 0): Record<number, any> {
    const styles: Record<number, any> = {};
    headers.forEach((header, index) => {
      const lowerHeader = header.toLowerCase();
      const colIndex = index + offset;
      // Center align status, code columns
      if (lowerHeader.includes('status') || lowerHeader.includes('code')) {
        styles[colIndex] = { halign: 'center' };
      }
      // Right align numeric columns
      if (lowerHeader.includes('amount') || lowerHeader.includes('price') || lowerHeader.includes('total')) {
        styles[colIndex] = { halign: 'right' };
      }
    });
    return styles;
  }

  // ==================== EXCEL EXPORT ====================

  /**
   * Export data to Excel/CSV
   */
  private async exportToExcel(
    data: ExportData[],
    filename: string,
    extension: ExcelExtension
  ): Promise<void> {
    const xlsx: typeof XLSX = await import('xlsx');

    const worksheet = xlsx.utils.json_to_sheet(data);

    // Auto-adjust column widths
    const headers = Object.keys(data[0]);
    worksheet['!cols'] = headers.map((header) => {
      const maxLength = Math.max(
        header.length,
        ...data.map((row) => String(row[header] ?? '').length)
      );
      return { wch: Math.min(maxLength + 2, 50) }; // Cap at 50 characters
    });

    const workbook: XLSX.WorkBook = {
      Sheets: { Sheet1: worksheet },
      SheetNames: ['Sheet1'],
    };

    const excelBuffer = xlsx.write(workbook, {
      bookType: extension,
      type: 'array',
    });

    await this.saveAsFile(excelBuffer, filename, extension);
  }

  /**
   * Export to Excel with styled headers (uses xlsx-js-style)
   * Includes title row and colored header
   */
  async exportToStyledExcel(
    data: ExportData[],
    options: ExportOptions
  ): Promise<void> {
    const xlsx = await import('xlsx-js-style') as unknown as typeof XLSX;

    const headers = Object.keys(data[0]);
    const title = options.title || options.filename;
    const headerColor = options.headerColor || '7C3AED'; // Default purple

    // Create worksheet data with title row
    const wsData: any[][] = [];

    // Row 1: Title (merged across all columns)
    wsData.push([title, ...Array(headers.length - 1).fill('')]);

    // Row 2: Empty row for spacing
    wsData.push(Array(headers.length).fill(''));

    // Row 3: Headers
    wsData.push(headers);

    // Data rows
    data.forEach(row => {
      wsData.push(headers.map(h => row[h] ?? ''));
    });

    const worksheet = xlsx.utils.aoa_to_sheet(wsData);

    // Merge title row
    worksheet['!merges'] = [
      { s: { r: 0, c: 0 }, e: { r: 0, c: headers.length - 1 } }
    ];

    // Style: Title row (row 0)
    const titleCell = worksheet[xlsx.utils.encode_cell({ r: 0, c: 0 })];
    if (titleCell) {
      (titleCell as any).s = {
        font: { bold: true, sz: 14, color: { rgb: '000000' } },
        alignment: { horizontal: 'center', vertical: 'center' },
        fill: { fgColor: { rgb: 'F3F4F6' } },
      };
    }

    // Style: Header row (row 2)
    headers.forEach((_, index) => {
      const cellAddress = xlsx.utils.encode_cell({ r: 2, c: index });
      const cell = worksheet[cellAddress];
      if (!cell) return;

      (cell as any).s = {
        font: { bold: true, color: { rgb: 'FFFFFF' }, sz: 11 },
        alignment: { horizontal: 'center', vertical: 'center' },
        fill: { fgColor: { rgb: headerColor } },
        border: {
          top: { style: 'thin', color: { rgb: '000000' } },
          bottom: { style: 'thin', color: { rgb: '000000' } },
          left: { style: 'thin', color: { rgb: '000000' } },
          right: { style: 'thin', color: { rgb: '000000' } },
        },
      };
    });

    // Style: Data rows with borders and alternating colors
    for (let r = 3; r < wsData.length; r++) {
      const isEvenRow = (r - 3) % 2 === 0;
      headers.forEach((_, c) => {
        const cellAddress = xlsx.utils.encode_cell({ r, c });
        const cell = worksheet[cellAddress];
        if (!cell) return;

        (cell as any).s = {
          font: { sz: 10 },
          alignment: { vertical: 'center' },
          fill: { fgColor: { rgb: isEvenRow ? 'FFFFFF' : 'F9FAFB' } },
          border: {
            top: { style: 'thin', color: { rgb: 'E5E7EB' } },
            bottom: { style: 'thin', color: { rgb: 'E5E7EB' } },
            left: { style: 'thin', color: { rgb: 'E5E7EB' } },
            right: { style: 'thin', color: { rgb: 'E5E7EB' } },
          },
        };
      });
    }

    // Auto-adjust column widths
    worksheet['!cols'] = headers.map((col) => {
      const maxLength = Math.max(
        col.length,
        title.length / headers.length,
        ...data.map((row) => String(row[col] ?? '').length)
      );
      return { wch: Math.min(maxLength + 4, 50) };
    });

    // Set row heights
    worksheet['!rows'] = [
      { hpt: 28 }, // Title row
      { hpt: 10 }, // Spacer row
      { hpt: 22 }, // Header row
    ];

    const workbook: XLSX.WorkBook = {
      Sheets: { [title]: worksheet },
      SheetNames: [title],
    };

    const excelBuffer = xlsx.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    await this.saveAsFile(excelBuffer, options.filename, 'xlsx');
  }

  // ==================== FILE SAVE ====================

  /**
   * Save buffer as file
   */
  private async saveAsFile(
    buffer: ArrayBuffer,
    filename: string,
    extension: string
  ): Promise<void> {
    const { saveAs } = await import('file-saver');

    const mimeTypes: Record<string, string> = {
      xlsx: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8',
      csv: 'text/csv;charset=UTF-8',
    };

    const blob = new Blob([buffer], {
      type: mimeTypes[extension] ?? 'application/octet-stream',
    });

    const timestamp = new Date().getTime();
    const finalFilename = `${filename}_export_${timestamp}.${extension}`;

    saveAs(blob, finalFilename);
  }

  // ==================== UTILITY METHODS ====================

  /**
   * Download blob as file (for API-returned blobs)
   */
  downloadBlob(blob: Blob, filename: string, extension = 'pdf'): void {
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename.endsWith(`.${extension}`)
      ? filename
      : `${filename}.${extension}`;
    link.click();
    window.URL.revokeObjectURL(url);
  }

  /**
   * Convert data to CSV string
   */
  toCSV(data: ExportData[]): string {
    if (!data || data.length === 0) return '';

    const headers = Object.keys(data[0]);
    const csvRows = [
      headers.join(','),
      ...data.map((row) =>
        headers
          .map((header) => {
            const value = row[header];
            const stringValue = String(value ?? '');
            // Escape quotes and wrap in quotes if contains comma
            if (stringValue.includes(',') || stringValue.includes('"')) {
              return `"${stringValue.replace(/"/g, '""')}"`;
            }
            return stringValue;
          })
          .join(',')
      ),
    ];

    return csvRows.join('\n');
  }
}