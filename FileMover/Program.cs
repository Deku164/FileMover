using System;
using System.Collections.Generic;
using System.IO;

namespace FileMover
{
    class Program
    {
        static string PATH_files = "";
        static string PATH_Absolute = "";
        static string seperator = "";

        static Dictionary<string, List<FileInfo>> Files = new Dictionary<string, List<FileInfo>>();

        static void Main(string[] args)
        {
            Console.WriteLine("파일들이 있는 경로를 풀로 적어주세요. (복사 붙여넣기 하시면 됩니다.)");
            PATH_files = Console.ReadLine();

            Console.WriteLine("입력한 파일 경로: " + PATH_files + "\n");

            Console.WriteLine("파일 이름을 자르는 기준을 알려주세요. ex) . _ / ");
            seperator = Console.ReadLine();

            Console.WriteLine("폴더들이 있는 경로를 풀로 적어주세요. (복사 붙여넣기 하시면 됩니다.)");
            PATH_Absolute = Console.ReadLine();

            Console.WriteLine("입력한 폴더 경로: " + PATH_Absolute + "\n");
            Console.WriteLine("파일들을 모두 읽어옵니다..");

            FileSearch();
            FileMove();

            Console.WriteLine("프로그램을 종료하시려면 아무키나 눌러주세요...");
            Console.ReadKey();
        }

        static void FileSearch()
        {
            DirectoryInfo dir = new DirectoryInfo(PATH_files);
            string fi = string.Empty;

            Console.WriteLine(seperator + " 로 글자를 나누었을때, 몇번째 글자가 폴더 이름인가요? ex) 0,1,2...");
            int _fileIndex = int.Parse(Console.ReadLine());

            foreach (System.IO.FileInfo file in dir.GetFiles())
            {
                string fileKey = file.Name.Split(char.Parse(seperator))[_fileIndex];
                if (Files.ContainsKey(fileKey))
                {
                    //키 보유 = 추가
                    List<FileInfo> li = Files[fileKey];
                    li.Add(file);
                }
                else
                {
                    //키 미 보유 = 생성
                    List<FileInfo> newList = new List<FileInfo>();
                    newList.Add(file);

                    Files.Add(fileKey, newList);
                }
            }

            foreach (var item in Files.Keys)
            {
                fi += item + " ";
            }
            Console.Clear();
            Console.WriteLine("찾은 파일 종류들... " + fi);
        }
        static void FileMove()
        {
            DirectoryInfo dir = new DirectoryInfo(PATH_Absolute);
            DirectoryInfo[] dirs = dir.GetDirectories("*",System.IO.SearchOption.AllDirectories);

            List<DirectoryInfo> dirList = new List<DirectoryInfo>();

            for (int i = 0; i < dirs.Length; i++)
            {
                dirList.Add(dirs[i]);
            }

            foreach (var item in Files)
            {
                DirectoryInfo _dir = new DirectoryInfo(PATH_Absolute + "\\" + item.Key);
                if (!_dir.Exists)                
                {
                    //폴더 없으면 생성 후 무브
                    _dir.Create();
                    Console.WriteLine("폴더가 없어 생성했습니다. 폴더이름: " + item.Key);
                }

                for (int i = 0; i < item.Value.Count; i++)
                {
                    File.Move(item.Value[i].FullName, _dir.FullName + "\\" + item.Value[i].Name);
                }
            }

            Console.WriteLine("파일을 모두 이동시켰습니다... ");
        }
    }
}
