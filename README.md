⚡ Lưu ý quan trọng
Không commit thư mục Library/ vì nó chứa cache, có thể tái tạo.

Luôn commit file .meta để tránh mất liên kết asset.

Sử dụng Git LFS (Large File Storage) cho file lớn (texture, audio, video).

bash
git lfs install
git lfs track "*.psd" "*.mp4" "*.wav"
📊 Quy trình chuẩn cho teamwork
Pull trước khi làm việc để cập nhật code mới nhất.

Commit nhỏ, có ý nghĩa (ví dụ: "Add player movement script").

Code review qua Pull Request để tránh lỗi.

Resolve conflict bằng cách merge thủ công trong Git hoặc Unity.
____________

Dưới đây là quy trình dựng game Unity 2D co-op online (2 người chơi / 2 máy khác nhau) kiểu Fireboy & Watergirl nhưng là Wind & Earth, chia việc để 5 thành viên ai cũng vừa thiết kế vừa viết C#.

0) Chọn stack kỹ thuật (để chơi khác máy, không local co-op)
Khuyến nghị (Unity chính chủ):

Netcode for GameObjects (NGO) + Unity Lobby + Unity Relay (dễ làm co-op nhỏ, xuyên NAT) 

Tham khảo mẫu co-op “Boss Room” để học pattern netcode 

(Mirror/FishNet cũng được, nhưng nếu muốn đi “đúng đường Unity Services” thì NGO+Relay+Lobby là combo rõ ràng nhất.) 

Mục tiêu mạng cho game puzzle 2 người:

1 người Host, người kia Client (host-authoritative cho logic puzzle).

Đồng bộ: vị trí nhân vật, trạng thái công tắc/cửa, vật thể tương tác, trạng thái hazard.

1) Thiết kế “core loop” & bộ khả năng Wind/Earth (1–2 ngày)
Cả team thống nhất 1 trang “Game Pillars”:

Phối hợp + timing (bắt buộc 2 người).

Mỗi nhân vật tác động môi trường khác nhau, và level luôn có “khóa” yêu cầu kết hợp.

Online co-op: vào phòng → chọn nhân vật → chơi.

Gợi ý ability set (đủ làm puzzle đa dạng):

Wind

Gust Push: thổi đẩy vật nhẹ, bật công tắc gió, đẩy platform trôi.

Glide/Updraft: tạo luồng gió nâng (đẩy bản thân/đồng đội lên vùng nhỏ).

Earth

Raise Pillar: dựng cột đất làm bệ đứng/đỡ vật nặng.

Rock Pull/Anchor: kéo vật nặng / neo giữ platform để gió không thổi trôi.

2) Chia module để 5 người đều “design + code”
Nguyên tắc: mỗi người sở hữu 1 subsystem gameplay + tự thiết kế 2–3 màn “khoe” subsystem đó.

Thành viên A — Producer/Integrator (vẫn có code)
Design: chuẩn hóa format level, đặt tiêu chí “mỗi màn dạy 1 ý”.
Code: khung dự án + Scene flow:

MainMenu → Lobby → Gameplay → Result

Hệ thống GameState (đang chơi/respawn/thắng/thua)

Build pipeline + versioning (SemVer), checklist QA.

Thành viên B — Wind Gameplay Owner
Design: bộ câu đố xoay quanh gió (đẩy vật, luồng nâng, timing).
Code:

WindAbilityController (input → cast → effect)

Vật thể phản ứng gió: Pushable, WindVolume, FanSwitch

Đồng bộ netcode cho ability (ServerRpc/ClientRpc hoặc NetworkObject + event).

Thành viên C — Earth Gameplay Owner
Design: puzzle dựng trụ, chặn hazard, tạo đường đi.
Code:

EarthAbilityController

PillarSpawner (spawn/despawn có giới hạn, cooldown)

HeavyPushable / AnchorPoint (đồ nặng chỉ Earth kéo/neo)

Thành viên D — Puzzle Systems Owner (các “đồ nghề” của level designer)
Design: template puzzle (cửa–công tắc–timer–checkpoint).
Code:

Interactable interface (On/Off/Use)

PressurePlate, Lever, TimedGate, DoorController

PuzzleGraph/Link (kéo thả trong Inspector: plate → door → trap)

Module này giúp làm level nhanh và đồng nhất.

Thành viên E — UI/UX + Online Session Owner (vẫn có code)
Design: luồng UX: tạo phòng, join bằng code, chọn nhân vật, ping/đứt mạng.
Code:

Lobby/Relay integration (create/join, hiển thị code phòng) 

UI in-game: emote/ping (để phối hợp), restart vote

Minimal tutorial prompts theo màn.

3) Kiến trúc project Unity (để đỡ “đạp chân nhau”)
Folder chuẩn:

Scripts/Core (GameState, SceneLoader)

Scripts/Net (NetworkBootstrap, LobbyRelay)

Scripts/Characters/Wind, Scripts/Characters/Earth

Scripts/Interactables (Plate, Door, TimedGate…)

Levels/Level01..

Prefabs/Networked/ (mọi thứ spawn qua net)

Coding conventions nhanh:

Mọi object cần sync → có NetworkObject.

Logic puzzle chạy server-authoritative:

Client gửi “ý định” (interact/ability) → Server xử lý → server bật NetworkVariable / gửi ClientRpc cập nhật.

Chỉ sync thứ cần thiết (tránh spam NetworkTransform cho mọi thứ).

4) Lộ trình làm theo mốc (đảm bảo ra game được)
Milestone 1 — Prototype offline (3–5 ngày)
2 nhân vật chạy/nhảy cơ bản

2 ability “thô”

3 interactable cơ bản: Door, Plate, Lever

1 level test “đủ win condition”

Milestone 2 — Vertical Slice online (5–7 ngày)
NGO chạy host/client, spawn đúng 2 nhân vật

1 phòng chơi được qua Relay/Lobby 

1 level hoàn chỉnh (UI tối thiểu, thắng/thua, respawn)

Milestone 3 — Content (1–2 tuần)
Mỗi người ship 2–3 level theo subsystem mình

Thêm 2–3 hazard (cát lún, đá rơi, gió ngược…)

Playtest chéo, sửa độ khó

Milestone 4 — Polish + QA (1 tuần)
UX online (mất kết nối, host thoát, retry)

Tối ưu sync, sửa bug “desync”

Âm thanh/feedback (VFX nhỏ, rung cam, highlight interact)

5) Cách phối hợp Git để 5 người làm song song
Branch theo feature: feature/wind-abilities, feature/puzzle-system…

Không ai sửa scene chính trực tiếp: mỗi level là 1 scene riêng.

Prefab/network prefab do “Integrator” duyệt (tránh lệch cấu hình NetworkObject).

Nếu bạn muốn, mình có thể phác luôn:

Danh sách class C# tối thiểu (tên file + trách nhiệm) cho từng module,

Sơ đồ RPC/NetworkVariable cho Door/Plate/Ability,

Và mẫu 1 level tutorial (Level01 dạy phối hợp + timing) để team bám theo.
