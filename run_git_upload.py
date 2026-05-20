import subprocess
import os
import shutil

log_file = r"D:\work\git_upload_result.txt"
folder = r"D:\work\上傳到公司"
repo_url = "https://github.com/we77465/my-app.git"
git = r"C:\Program Files\Git\cmd\git.exe"

with open(log_file, 'w', encoding='utf-8') as f:
    f.write("start\n")

    # 移除損壞的 .git
    git_dir = os.path.join(folder, '.git')
    if os.path.exists(git_dir):
        shutil.rmtree(git_dir, ignore_errors=True)
        f.write("removed old .git\n")

    os.chdir(folder)
    f.write(f"cwd: {os.getcwd()}\n")

    def run(cmd):
        result = subprocess.run(
            [git] + cmd,
            capture_output=True, text=True, encoding='utf-8', errors='replace'
        )
        f.write(f"$ git {' '.join(cmd)}\n")
        if result.stdout: f.write(f"  {result.stdout.strip()}\n")
        if result.stderr: f.write(f"  STDERR: {result.stderr.strip()}\n")
        f.write(f"  returncode: {result.returncode}\n")
        return result

    run(["init", "-b", "main"])
    run(["config", "user.email", "kenandme87@gmail.com"])
    run(["config", "user.name", "ken"])
    run(["remote", "add", "origin", repo_url])
    run(["add", "."])
    run(["commit", "-m", "新增 CHBApp 檢核比對報告"])
    run(["push", "-u", "origin", "main", "--force"])

    f.write("DONE\n")
