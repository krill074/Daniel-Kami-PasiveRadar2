# 📡 PassiveRadar2 – Enhanced Fork by @krill074

**PassiveRadar2** is a C# WinForms passive radar application designed to process radio reflections using broadcast signals and RTL-SDR dongles. This fork focuses on fixing critical stability issues, particularly a **silent crash** caused by CUDA runtime mismatches and major UI bugs in `RadarControl.cs`.

---

## 💻 System Environment

* **Operating System:** Windows 10 Pro x64 (Build 19045)
* **Visual Studio:** 2022 (latest version with `.NET desktop development` workload)
* **CUDA Version:** 10.2 (installed manually from NVIDIA archive)
* **Hardware:** Single RTL-SDR dongle
* **Note:** No Linux, Docker, or WSL was used. Native Windows environment only.

---

## 🧽 Timeline of Issues & Fixes

### ✅ Stage 1: Initial Setup

* Forked and cloned Daniel Kamiński's `PasiveRadar2` repository.
* Code loaded successfully in Visual Studio 2022.

---

### ❌ Stage 2: Silent Application Crash

**Issue:**
App failed silently at startup — no errors or logs.

**Cause:**
`Ambiguity.dll` failed internally due to a CUDA mismatch.
DLL was built against `cuFFT` from CUDA 10.2, but system had a newer version.

**Fix:**
Installed [CUDA Toolkit 10.2](https://developer.nvidia.com/cuda-10.2-download-archive)
Verified correct DLL (`cufft64_10.dll`) was available.

**Result:**
Application launched without crashing.

---

### ⚠️ Stage 3: UI Crash from TrackBar.Value

**Issue:**
Exception from out-of-range slider values:

```
System.ArgumentOutOfRangeException: 'Value of 'X' is not valid for 'Value'.'
```

**Fix:**
Added this helper in `RadarControl.cs`:

```
private int Clamp(int value, int min, int max)
    => Math.Max(min, Math.Min(value, max));
```

Wrapped all `TrackBar.Value = x` assignments with `Clamp()`.

**Result:**
No more UI crashes; sliders safe to use.

---

### 🔁 Stage 4: Control Conflict (`trackBar1` reused)

**Issue:**
`trackBar1` was reused for both `Columns` and `BufferSize`, causing conflict.

**Fix:**
Created a new control: `trackBar11` for `BufferSize`.
Separated all logic for the two sliders.

**Result:**
UI controls now function independently.

---

### 🧪 Stage 5: Final Runtime Status

* ✅ App builds and runs without crash
* ✅ All sliders respond correctly
* ✅ `Ambiguity.dll` executes using CUDA 10.2
* 🔍 **No confirmed radar output yet** — app is stable, but no visual radar detected

---

## 🗾️ Summary Table

| Issue                                | Status  | Fix Implemented                          |
| ------------------------------------ | ------- | ---------------------------------------- |
| Silent crash from `Ambiguity.dll`    | ✅ Fixed | Installed CUDA 10.2                      |
| TrackBar exceptions (UI crash)       | ✅ Fixed | Wrapped all values with `Clamp()`        |
| Control conflict (`trackBar1` reuse) | ✅ Fixed | Split into separate controls             |
| Radar output confirmation            | ⟳ TBD   | App runs, but radar map not observed yet |

---

## 🙌 Credits

* Original project by [Daniel Kamiński](https://github.com/DanielKami)
* Fork, fixes, and documentation by [@krill074](https://github.com/krill074)
