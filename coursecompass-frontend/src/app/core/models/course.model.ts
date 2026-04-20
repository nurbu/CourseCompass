export interface Course {
  id: number;
  code: string;
  title: string;
  credits: number;
  description: string;
  department: string;
  prerequisites: string[];
}
