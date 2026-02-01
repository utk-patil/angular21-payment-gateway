import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'localDateTime',
  standalone: true,
})
export class LocalDateTimePipe implements PipeTransform {

  transform(value: string | Date | null | undefined, locale: string = 'en-IN'): string {

        if (!value) return '-';

        const iso = typeof value === 'string' && !value.endsWith('Z') ? value + 'Z' : value;

        const date = new Date(iso);

        return new Intl.DateTimeFormat(locale, {
            day: '2-digit',
            month: 'short',
            year: 'numeric',
            hour: 'numeric',
            minute: '2-digit',
            hour12: true,
            timeZone: 'Asia/Kolkata',
        }).format(date);
    }

}
