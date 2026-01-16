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
  </system.webServer>
</configuration>`;

// Adjust path based on your output directory
const outputPath = path.join(__dirname, 'dist', 'your-app-name', 'web.config');

fs.writeFileSync(outputPath, webConfig);
console.log('✅ web.config generated successfully!');
