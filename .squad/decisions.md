# Decisions

- 2026-02-27: Squad initialized for k6-aspire-hosting with Alien-universe cast names.
- 2026-02-27: Worktree-local .squad state is used at repository root.

## 2026-02-27: Adoption Strategy (Ripley)
**Status:** APPROVED  
**Owner:** Kori Francis (with Ripley oversight)

**Decision:** Pursue immediate Week 1–2 low-friction adoption wins.

**Top 10 Opportunities (Prioritized):**
1. Publish curated Grafana dashboards to k6 marketplace (High impact, Low effort)
2. Create end-to-end video tutorials: 5–10 min each (High impact, Medium effort)
3. Add GitHub Actions starter template & workflow (High impact, Medium effort)
4. Publish "ASP.NET Core Load Testing Playbook" (Medium impact, Medium effort)
5. Contribute to official Grafana k6 xk6 extension ecosystem (Medium impact, High effort)
6. Launch annual "Benchmark Report" (Medium impact, Medium effort)
7. Create integration templates: Datadog, New Relic, Splunk (Medium impact, Medium effort)
8. Host monthly "Load Testing Office Hours" (Medium impact, Medium effort)
9. Publish microservices sample (3-service mesh scenario) (Low impact, Medium effort)
10. Partner with Microsoft Learn & official Aspire docs (Low impact, High effort)

**30-Day Execution Plan:**
- Week 1: Audit dashboards, draft GitHub Actions workflow, record first tutorial
- Week 2: Publish playbook, submit dashboards to k6 Marketplace, record video #2, blog post
- Week 3: Multi-service samples, vendor integration templates, video #3, Office Hours launch
- Week 4: Benchmark Report v1, README refresh, social campaign, feedback collection

**North-Star Metrics (12-month targets):**
- NuGet downloads: ~50 → ~600 (monthly avg)
- GitHub stars: 9 → 100+
- Active users (inferred): <10 → ~75

**Next Phase:** Evaluate Q1 results before committing to xk6/partnership work.

---

## 2026-02-27: High-Value Example Additions (Parker)
**Status:** APPROVED  
**Owner:** Kori Francis (with Parker guidance on prioritization)

**Decision:** Implement 8 high-value examples in phased rollout.

**Tier 1: High Star Potential + Medium Complexity**
1. Multi-Stage Authentication Load Test (OAuth/JWT flows)
2. E-Commerce Checkout Flow (browse → checkout → payment)
3. Resilience Patterns & Circuit Breaker (failure modes under stress)

**Tier 2: High Star Potential + Deep Investment**
4. Database Query Stress Under Load (connection pools, N+1 detection)
5. WebSocket/Real-Time Streaming (chat, notifications, message ordering)

**Tier 3: Medium Star Potential + Medium Complexity**
6. GraphQL Query Complexity Under Load (nested queries, mutations, subscriptions)
7. Multi-Region/Geographic Load Distribution (latency injection, failover)

**Tier 4: Quick Wins**
8. Smoke Tests + Baseline Detection (CI/CD pipeline integration)

**Phased Implementation Roadmap:**
- **Phase 1 (Weeks 1–2):** Smoke tests + simplified Auth flow (quick wins)
- **Phase 2 (Weeks 3–6):** E-Commerce checkout, Resilience patterns, GraphQL
- **Phase 3 (Weeks 7+):** WebSocket/Real-Time, Database stress, Multi-Region

**Top 3 Viral Candidates:**
- E-Commerce Checkout (high relatable appeal)
- WebSocket/Real-Time (fills unique gap, differentiates vs. competitors)
- Resilience Patterns (perfect Aspire ecosystem synergy)

**Success Metrics:**
- GitHub stars gained per new example
- NuGet downloads trending
- Social mentions / shares
- Community issues/PRs per example
- "See also" links in Grafana k6 docs

---

## 2026-02-27: Documentation Funnel & SEO Strategy (Lambert)
**Status:** IN PROGRESS  
**Owner:** Kori Francis (with Lambert guidance on funnel design)

**Decision:** Integrate docs improvements into Ripley's content roadmap and Parker's example progression.

**Expected Coordinations:**
- ASP.NET Core Load Testing Playbook (feeds from Ripley Week 2 content)
- Example-specific tutorial documentation (feeds from Parker's phased examples)
- Video companion guides (align with 3 × 5-min video series from Ripley)
- CI/CD integration docs (GitHub Actions from Ripley's Week 1)

**Funnel Improvements (In Development):**
- SEO optimization for "k6 aspire", "aspire load testing"
- Navigation hierarchy improvements
- Beginner → Intermediate → Advanced progression
- Organic discovery strategy alignment

---

## 2026-02-27: 4-Week Execution Backlog (Ripley)
**Status:** APPROVED  
**Owner:** Ripley (lead oversight)  

**Decision:** Execute structured 30-day adoption sprint with 14 sequenced tasks (WK1-1 through WK4-4).

**Structure:**
- **Week 1:** Foundation & quick wins (dashboards, CI/CD, video)
- **Week 2:** Market & authority (playbook, marketplace, feedback)
- **Week 3:** Breadth & ecosystem (multi-service, integrations, office hours)
- **Week 4:** Amplify & measure (benchmark, README, social, retrospective)

**Team Allocation:** ~158 hours (Ripley lead, Dallas/Parker/Ash/Lambert/Bishop)

**North-Star Metrics (12-month):**
- NuGet downloads: 50 → 600
- GitHub stars: 9 → 100+
- Active users: <10 → 75+

**Weekly Pulse:** Mondays (Ripley)

**Exit Criteria (EOW4):**
- Green: NuGet ≥100/mo, Stars ≥20, ≥5 Discussions threads, ≥2 external adoption signals → Q1 Tier 2
- Yellow: Moderate metrics, reassess roadmap
- Red: Flat/declining metrics, negative feedback → pivot to docs + community focus

**Risk Mitigations:** Video quality, CI edge cases, sample complexity, marketplace rejection, social engagement, capacity overrun

---

## 2026-02-27: README/Docs Quick Wins (Lambert)
**Status:** COMPLETE  
**Owner:** Lambert  

**Decision:** Execute low-effort, high-impact docs improvements to reduce onboarding friction.

**Scope:**
- Strengthened README value proposition (top-line)
- Reordered Quick Start into logical sequence
- Corrected package/doc link mismatches (Outputs.Aspire)
- Added "Next steps" sample progression

**Impact:** Improved GitHub landing funnel, reduced cognitive load for new users, prepared foundation for video tutorials.

---

## 2026-02-27: GrafanaInfluxDbSample Smoke Test (Parker)
**Status:** COMPLETE  
**Owner:** Parker  

**Decision:** Enhance sample with rapid validation path for new users.

**Scope:**
- Added smoke profile support in `scripts/test.js` (via `TEST_PROFILE=smoke`)
- Enabled script path switching via `K6_SCRIPT` environment variable
- No API/package changes—minimal risk surface

**Impact:** Users can validate setup in <5 minutes, supports Lambert's docs funnel + Ripley's video tutorials, foundation for Tier 2 examples.

---
