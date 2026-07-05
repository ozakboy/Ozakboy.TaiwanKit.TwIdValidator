<script setup lang="ts">
// JSON-LD 結構化資料 — 讓 Google 認得這是 SoftwareApplication
const jsonLd = {
  '@context': 'https://schema.org',
  '@type': 'SoftwareApplication',
  name: 'Ozakboy.TaiwanKit.TwIdValidator',
  alternateName: ['TwIdValidator', 'Taiwan ID Validator for .NET'],
  applicationCategory: 'DeveloperApplication',
  applicationSubCategory: 'Validation Library',
  operatingSystem: 'Cross-platform (.NET Standard 2.0+, .NET 8/9/10)',
  description:
    'Taiwan national ID, business administration number (BAN) and mobile phone number validator for .NET. Covers all three kinds of Taiwan ID numbers, the April 2021 BAN checksum rule, phone normalization and checksum-valid test-data generation. Zero runtime dependencies, thread-safe.',
  url: 'https://ozakboy.github.io/Ozakboy.TaiwanKit.TwIdValidator/',
  downloadUrl: 'https://www.nuget.org/packages/Ozakboy.TaiwanKit.TwIdValidator/',
  image: 'https://ozakboy.github.io/Ozakboy.TaiwanKit.TwIdValidator/logo.png',
  author: {
    '@type': 'Person',
    name: 'ozakboy',
    url: 'https://github.com/ozakboy',
  },
  license: 'https://opensource.org/licenses/MIT',
  programmingLanguage: 'C#',
  offers: {
    '@type': 'Offer',
    price: '0',
    priceCurrency: 'USD',
  },
  softwareVersion: '1.0.1',
}

useHead({
  script: [
    {
      type: 'application/ld+json',
      innerHTML: JSON.stringify(jsonLd),
    },
  ],
})
</script>

<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>
