// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-01-01',
  devtools: { enabled: true },

  // Nuxt site config (used by sitemap module to prepend absolute URL)
  // 注意:url 只能放 domain,baseURL 在 app.baseURL 處理(GitHub Pages project site 規則)
  site: {
    url: 'https://twid.ozakboy.life',
    name: 'Ozakboy.TaiwanKit.TwIdValidator',
    description:
      'Taiwan national ID, business administration number (BAN) and mobile phone number validator for .NET. Detailed parse results, input normalization and test-data generation. Zero runtime dependencies.',
  },

  app: {
    baseURL: '/',
    head: {
      title: 'TwIdValidator — 台灣身分證/統編/手機驗證 .NET 套件',
      htmlAttrs: { lang: 'zh-TW' },
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
        {
          name: 'description',
          content:
            'Ozakboy.TaiwanKit.TwIdValidator — Taiwan ID / BAN / mobile number validator for .NET. Covers national ID, new/old resident certificate numbers, the 2021 BAN rule, phone normalization and test-data generation. Zero dependencies.',
        },
        {
          name: 'keywords',
          content:
            'Taiwan ID validator, 身分證驗證, 統一編號驗證, 手機號碼驗證, TwIdValidator, TaiwanKit, .NET, csharp, NuGet, dotnet, national id, BAN',
        },
        { name: 'author', content: 'ozakboy' },

        // Search engine verification
        {
          name: 'google-site-verification',
          content: '7B6Z2O-JFfFD6I0jeayWg1SFeDWKZmf4RwSVGbQHmVk',
        },
        { name: 'msvalidate.01', content: '4928B4223346F74DB53D9754C37164AB' },

        // Open Graph (Facebook / LinkedIn / general social)
        { property: 'og:type', content: 'website' },
        { property: 'og:site_name', content: 'Ozakboy.TaiwanKit.TwIdValidator' },
        { property: 'og:title', content: 'TwIdValidator — Taiwan ID / BAN / Mobile Validator for .NET' },
        {
          property: 'og:description',
          content:
            'Validate Taiwan national IDs (all three kinds), BAN (2021 rule) and mobile numbers. Detailed parse results, normalization, test-data generation. Zero dependencies.',
        },
        { property: 'og:url', content: 'https://twid.ozakboy.life/' },
        { property: 'og:image', content: 'https://twid.ozakboy.life/logo.png' },
        { property: 'og:image:alt', content: 'Ozakboy.TaiwanKit logo' },
        { property: 'og:locale', content: 'zh_TW' },
        { property: 'og:locale:alternate', content: 'en_US' },

        // Twitter Card
        { name: 'twitter:card', content: 'summary' },
        { name: 'twitter:title', content: 'TwIdValidator — Taiwan ID / BAN / Mobile Validator for .NET' },
        {
          name: 'twitter:description',
          content:
            'Validate Taiwan national IDs, BAN and mobile numbers. Zero dependencies, thread-safe, test-data generation included.',
        },
        { name: 'twitter:image', content: 'https://twid.ozakboy.life/logo.png' },
      ],
      link: [
        { rel: 'icon', type: 'image/png', href: '/logo.png' },
        { rel: 'apple-touch-icon', href: '/logo.png' },
        { rel: 'canonical', href: 'https://twid.ozakboy.life/' },
      ],
    },
  },

  modules: [
    '@nuxtjs/i18n',
    '@nuxtjs/tailwindcss',
    '@nuxt/content',
    '@nuxtjs/sitemap',
    // @nuxtjs/robots 不適用(Project Pages baseURL 與 robots.txt root 衝突),
    // 改用 site/public/robots.txt 靜態檔
  ],

  // @nuxt/content 預設讀 site/content/。內容由 scripts/sync-docs.mjs 自動從 ../docs/ 同步進來。
  content: {
    build: {
      markdown: {
        toc: { depth: 3 },
        highlight: {
          theme: 'github-light',
        },
      },
    },
  },

  i18n: {
    baseUrl: 'https://twid.ozakboy.life',
    locales: [
      { code: 'zh-TW', name: '繁體中文', file: 'zh-TW.json' },
      { code: 'en', name: 'English', file: 'en.json' },
    ],
    defaultLocale: 'zh-TW',
    strategy: 'prefix_except_default',
    langDir: 'locales/',
    detectBrowserLanguage: {
      useCookie: true,
      cookieKey: 'i18n_redirected',
      redirectOn: 'root',
    },
  },

  // Sitemap: 自動產 sitemap.xml
  sitemap: {
    autoLastmod: true,
  },

  // GitHub Pages 部署:使用內建 preset,自動產生 404.html / .nojekyll
  nitro: {
    preset: 'github_pages',
  },

  ssr: true,
})
