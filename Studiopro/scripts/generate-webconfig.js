const fs = require('fs');
const path = require('path');

const webConfig = `<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="Angular Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/index.html" />
        </rule>
      </rules>
    </rewrite>
    <staticContent>
      <mimeMap fileExtension=".json" mimeType="application/json" />
      <mimeMap fileExtension=".woff" mimeType="font/woff" />
      <mimeMap fileExtension=".woff2" mimeType="font/woff2" />
    </staticContent>
  </system.webServer>
</configuration>`;

// Output path for Angular application build
const outputPath = path.join(__dirname, '..', 'dist', 'studiopro', 'browser', 'web.config');

// Ensure the directory exists
const outputDir = path.dirname(outputPath);
if (!fs.existsSync(outputDir)) {
  console.log('Build output directory not found. Run "npm run build" first.');
  process.exit(1);
}

fs.writeFileSync(outputPath, webConfig);
console.log('web.config generated successfully at:', outputPath);
