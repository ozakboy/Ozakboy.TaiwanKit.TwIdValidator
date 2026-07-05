<script setup>
useHead({ title: 'TwIdValidator — 台灣身分證/統編/手機驗證 .NET 套件' })
const localePath = useLocalePath()

const features = [
  { key: 'threeIds', icon: '🪪' },
  { key: 'ban', icon: '🏢' },
  { key: 'phone', icon: '📱' },
  { key: 'generator', icon: '🧪' },
]
</script>

<template>
  <div>
    <!-- Hero -->
    <section class="bg-gradient-to-br from-brand-900 via-brand-700 to-brand-500 text-white">
      <div class="max-w-6xl mx-auto px-4 py-20 sm:py-28 text-center">
        <h1 class="text-4xl sm:text-6xl font-extrabold tracking-tight">
          {{ $t('home.hero.title') }}
        </h1>
        <p class="mt-6 text-lg sm:text-xl text-brand-50 max-w-3xl mx-auto leading-relaxed">
          {{ $t('home.hero.subtitle') }}
        </p>
        <div class="mt-10 flex justify-center gap-3 flex-wrap">
          <NuxtLink
            :to="localePath('/docs/getting-started')"
            class="bg-white text-brand-900 px-6 py-3 rounded font-semibold hover:bg-brand-50 transition"
          >
            {{ $t('home.hero.install') }}
          </NuxtLink>
          <a
            href="https://github.com/ozakboy/Ozakboy.TaiwanKit.TwIdValidator"
            target="_blank"
            rel="noopener"
            class="bg-brand-900/40 text-white px-6 py-3 rounded font-semibold hover:bg-brand-900/60 border border-white/30 transition"
          >
            {{ $t('home.hero.github') }}
          </a>
        </div>
      </div>
    </section>

    <!-- Features -->
    <section class="max-w-6xl mx-auto px-4 py-16 sm:py-20">
      <h2 class="text-3xl sm:text-4xl font-bold text-center mb-12">
        {{ $t('home.features.title') }}
      </h2>
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        <div
          v-for="f in features"
          :key="f.key"
          class="bg-white border border-slate-200 rounded-lg p-6 hover:shadow-lg hover:border-brand-500 transition"
        >
          <div class="text-3xl mb-3">{{ f.icon }}</div>
          <h3 class="font-bold text-lg mb-2">
            {{ $t(`home.features.items.${f.key}.title`) }}
          </h3>
          <p class="text-slate-600 text-sm leading-relaxed">
            {{ $t(`home.features.items.${f.key}.desc`) }}
          </p>
        </div>
      </div>
    </section>

    <!-- Quick Start -->
    <section class="bg-white py-16 sm:py-20 border-t border-slate-200">
      <div class="max-w-4xl mx-auto px-4">
        <h2 class="text-3xl sm:text-4xl font-bold text-center mb-12">
          {{ $t('home.quickstart.title') }}
        </h2>
        <div class="space-y-8">
          <div>
            <h3 class="font-semibold text-lg mb-3">{{ $t('home.quickstart.step1') }}</h3>
            <pre class="bg-slate-900 text-slate-100 rounded p-4 text-sm overflow-x-auto"><code>dotnet add package Ozakboy.TaiwanKit.TwIdValidator</code></pre>
          </div>
          <div>
            <h3 class="font-semibold text-lg mb-3">{{ $t('home.quickstart.step2') }}</h3>
            <pre class="bg-slate-900 text-slate-100 rounded p-4 text-sm overflow-x-auto"><code>using Ozakboy.TaiwanKit.TwIdValidator;

TwId.IsValid("A123456789");          // true
TwBan.IsValid("04595257");           // true
TwPhone.IsValid("0912345678");       // true

TwIdResult r = TwId.Validate("A123456789");
// r.Kind → NationalId, r.Gender → Male, r.RegionName → "臺北市"</code></pre>
          </div>
          <div>
            <h3 class="font-semibold text-lg mb-3">{{ $t('home.quickstart.step3') }}</h3>
            <pre class="bg-slate-900 text-slate-100 rounded p-4 text-sm overflow-x-auto"><code>// 正規化 +886 / 分隔符 / 全形數字
TwPhone.TryNormalize("+886-912-345-678", out string n);  // n = "0912345678"

// 產生檢核碼合法的測試假資料（不對應真實人物）
TwId.Generate(TwIdKind.NationalId, Gender.Female);
TwBan.Generate();</code></pre>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>
